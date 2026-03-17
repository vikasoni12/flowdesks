using Flowdesks.Application.Responses.BulkUpload;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Flowdesks.Application.Interfaces.Common;
public interface IBulkUploadService
{
    Task<List<BulkUploadGenericDatatableResponse<object>>> GetDataFromExcel(IFormFile file, CancellationToken Token);
}
