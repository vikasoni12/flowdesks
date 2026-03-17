using Flowdesks.Application.Interfaces.Email.IEmailPopulate;
using Microsoft.AspNetCore.Hosting;

namespace Flowdesks.Infrastructure.Services.Email.EmailPopulate;

public class EmailPopulateBody : IEmailPopulateBody
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public EmailPopulateBody(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }


    //public async Task<string> PopulateBody(string emailType)
    //{
    //    string body = string.Empty;

    //    var path = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", emailType).ToLower();
    //    using (StreamReader reader = new StreamReader(path))
    //    {
    //        body = reader.ReadToEnd();
    //    }
    //    return body;
    //}

    public async Task<string> PopulateBody(string emailType)
    {
        string body = string.Empty;

        var basePath = _webHostEnvironment.WebRootPath;

        var path = Path.Combine(basePath, "EmailTemplates", emailType);

        var fileExists = File.Exists(path);

        if (fileExists)
        {
            using (StreamReader reader = new(path))
            {
                body = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            return body;
        }

        return null;
    }
}
