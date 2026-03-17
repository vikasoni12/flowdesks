using Flowdesks.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Requests.BulkUpload;
public class BulkSiteRequest : IRequest<Result<int>>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PostCode { get; set; }
    public Guid? CountryId { get; set; }
    public string TelephoneNumber { get; set; }
    public string ContactNumber { get; set; }
    public UploadByteArray ProfilePicture { get; set; } = new();
}
