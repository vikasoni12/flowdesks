using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Requests.UploadFiles;
using Flowdesks.Application.Responses;
using Flowdesks.Application.Responses.UploadFiles;
using Flowdesks.Shared.Wrapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using System.Security.Policy;

namespace Flowdesks.Infrastructure.Services.Common
{
    public class UploadService : IUploadService
    {
        private static readonly Dictionary<string, List<byte[]>> uploadedChunks = new();
        private static readonly Dictionary<string, string> uploadedFiles = new();
        private static readonly object uploadLock = new();
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Result<UploadChunkResponse>> UploadAsync(UploadChunkRequest request)
        {
            var uploadChunkResponse = new UploadChunkResponse();
            string uploadSequence = string.Empty;
            var chunkMax = Convert.ToInt64(request.ChunkMax);
            var chunkIndex = Convert.ToInt64(request.ChunkIndex);
            var stream = request.File.OpenReadStream();
            var uploadId = request.UploadId;

            try
            {
                var lastPart = (chunkMax - chunkIndex) == 1;
                var partNumber = chunkIndex + 1;

                var ms = new MemoryStream();
                stream.CopyTo(ms);
                ms.Position = 0;

                //Step 1: upload each chunk (this is run for every chunk unlike the other steps which are run once)
                var chunkData = new byte[ms.Length];
                ms.Read(chunkData, 0, chunkData.Length);

                lock (uploadLock)
                {
                    if (chunkIndex == 0)
                    {
                        uploadId = Guid.NewGuid().ToString();
                        uploadSequence = "first";

                        uploadedChunks[uploadId] = new List<byte[]>(); // Initialize the list for the uploadId
                        uploadedFiles[uploadId + "_FileName"] = $"{request.FileName}";
                    }

                    uploadedChunks[uploadId].Add(chunkData);
                }


                //Step 2: build and send the multipart complete request
                if (lastPart)
                {
                    //Set the uploadId and fileURLs with the response.

                    byte[] fullFile;
                    string fileName;
                    lock (uploadLock)
                    {
                        // Combine all chunks to reconstruct the original file
                        fullFile = uploadedChunks[uploadId].SelectMany(chunk => chunk).ToArray();
                        fileName = uploadedFiles[uploadId + "_FileName"];

                        // Cleanup: Remove the uploaded chunks data from the dictionary
                        uploadedChunks.Remove(uploadId);
                        uploadedChunks.Remove(uploadId + "_FileName");
                    }

                    // Save the reconstructed file on the server
                    string wwwRootPath = _webHostEnvironment.WebRootPath + "/" + request.Path;

                    bool exists = Directory.Exists(wwwRootPath);

                    if (!exists)
                        Directory.CreateDirectory(wwwRootPath);

                    string finalFileName = uploadId + Path.GetExtension(fileName); // Customize the final file name
                    string filePath = Path.Combine(wwwRootPath, finalFileName);

                    // Save the file
                    await File.WriteAllBytesAsync(filePath, fullFile);

                    uploadChunkResponse = new UploadChunkResponse
                    {
                        Status = "success",
                        UploadId = uploadId,
                        Location = GetFilePath(filePath),
                        UploadSequence = "final"
                    };
                }
                else
                {
                    //Set the uploadId and eTags with the response
                    uploadChunkResponse = new UploadChunkResponse
                    {
                        Status = "success",
                        UploadId = uploadId,
                        Location = null,
                        UploadSequence = !string.IsNullOrEmpty(uploadSequence) ? "middle" : uploadSequence,
                    };
                }

                return Result<UploadChunkResponse>.Success(uploadChunkResponse);
            }
            catch (Exception ex)
            {
                uploadedChunks.Remove(uploadId);
                uploadedChunks.Remove(uploadId + "_FileName");
                return Result<UploadChunkResponse>.Fail();
            }
        }

        public async Task<Result<string>> UploadAsync(UploadRequest request)
        {
            try
            {
                var fileBytes = request.FileBytes;
                var uploadId = Guid.NewGuid().ToString();

                // Save the reconstructed file on the server
                string folder = _webHostEnvironment.WebRootPath + "/" + request.Path;

                bool exists = Directory.Exists(folder);

                if (!exists)
                    Directory.CreateDirectory(folder);

                string finalFileName = uploadId + Path.GetExtension(request.FileName); 
                string filePath = Path.Combine(folder, finalFileName);

                // Save the file
                await File.WriteAllBytesAsync(filePath, fileBytes);

                return Result<string>.Success(data: GetFilePath(filePath));
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message);
            }
        }

        public async Task<Result<int>> DeleteAsync(string url)
        {
            string fileName = _webHostEnvironment.WebRootPath + "/" + url;

            var fileExists = File.Exists(fileName);

            if (fileExists)
            {
                File.Delete(fileName);
                return await Result<int>.SuccessAsync("Document deleted successfully");
            }

            return await Result<int>.SuccessAsync("No Document Found");
        }

        private string GetFilePath(string fullPath)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            // Replace forward slashes with backslashes
            fullPath = fullPath.Replace("\\", "/");

            // Combine the wwwRootPath with the remaining path
            string relativePath = Path.Combine(wwwRootPath, fullPath);

            // Find the index of wwwroot in the file path
            int wwwrootIndex = relativePath.IndexOf(fullPath, StringComparison.OrdinalIgnoreCase);

            // Extract the path after wwwroot
            string pathAfterWwwroot = wwwrootIndex != -1 ? relativePath.Substring(wwwrootIndex + wwwRootPath.Length) : relativePath;

            return pathAfterWwwroot;
        }

        public FileDetails GetFileDetails(string fileName)
        {
            string filePath = _webHostEnvironment.WebRootPath + "/" + fileName;

            if (!File.Exists(filePath))
            {
                return null;
            }

            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            try
            {
                contentType = provider.Mappings.TryGetValue(Path.GetExtension(filePath), out var mappedContentType) ? mappedContentType : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting content type for {filePath}: {ex.Message}");
                contentType = null;
            }

            var fileInfo = new FileInfo(filePath);

            return new FileDetails
            {
                Name = fileInfo.Name,
                FullName = filePath,
                Length = fileInfo.Length,
                CreationTime = fileInfo.CreationTime,
                LastWriteTime = fileInfo.LastWriteTime,
                Extension = fileInfo.Extension,
                ContentType = contentType
            };
        }

        public async Task<Result<int>> DeleteManyAsync(List<string> urls)
        {
            try
            {
                foreach (string url in urls)
                {
                    //Delete document 
                    string fileName = _webHostEnvironment.WebRootPath + "/" + url;

                    var fileExists = File.Exists(fileName);

                    if (fileExists)
                    {
                        File.Delete(fileName);
                    }
                }

                return await Result<int>.SuccessAsync();
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex.Message);
            }
        }

    }
}
