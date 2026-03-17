using Flowdesks.Application.Responses.Site;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Site
{
    public class UpdateSiteRequest:IRequest<Result<SiteResponse>>
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public Guid? CountryId { get; set; }       
        public string? TelephoneNumber { get; set; }
        public string? ContactNumber { get; set; }

        public string? ProfilePictureUrl { get; set; }
        public UploadByteArray ImageName { get; set; } = new();
    }
}
