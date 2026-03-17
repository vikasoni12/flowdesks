using System.Drawing;

namespace Flowdesks.Shared.Utility;

public class GenrateImageBase64Function
{
    private static Image.GetThumbnailImageAbort _cancellationToken;

    public static string GetBase64(string imagePath)
    {
        if (imagePath == null) { return null; }

        var base64String = "";
        if (imagePath != "Files/UserProfileImage/" && !imagePath.Contains("Files/UserProfileImage/https://localhost:5001/Files/UserProfileImage/"))
        {
            if (File.Exists(imagePath))
            {
                using Image image = Image.FromFile(imagePath);
                double widthRatio = 50 / image.Width;
                double heightRatio = 50 / image.Height;
                double ratio = widthRatio < heightRatio ? widthRatio : heightRatio;
                using Image thumbnail = image.GetThumbnailImage((int)(image.Width * ratio), (int)(image.Height * ratio), _cancellationToken, nint.Zero);
                MemoryStream _mStream = new MemoryStream();
                byte[] _imageBytes = null;
                using (var memory = new MemoryStream())
                {
                    thumbnail.Save(memory, image.RawFormat);
                    _imageBytes = memory.ToArray();
                }

                //  thumbnail.Save(_mStream, thumbnail.RawFormat);

                base64String = Convert.ToBase64String(_imageBytes);
                base64String = "data:image/jpg;base64," + base64String;
            }
        }
        return base64String;

    }
}
