using Flowdesks.Application.Requests.UploadFiles;
using Flowdesks.Application.Responses;
using Flowdesks.Application.Responses.UploadFiles;
using Flowdesks.Shared.Wrapper;

namespace Flowdesks.Application.Interfaces.Common
{
    public interface IUploadService
    {
        Task<Result<UploadChunkResponse>> UploadAsync(UploadChunkRequest request);
        Task<Result<string>> UploadAsync(UploadRequest request);
        Task<Result<int>> DeleteAsync(string url);
        Task<Result<int>> DeleteManyAsync(List<string> urls);
        FileDetails GetFileDetails(string filePath);
    }
}