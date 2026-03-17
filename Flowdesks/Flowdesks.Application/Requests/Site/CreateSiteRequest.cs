using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Site
{
    public class CreateSiteRequest : IRequest<Result<int>>
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
}
