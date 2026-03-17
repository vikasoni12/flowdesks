using Flowdesks.Application.Responses.Site;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Site
{
    public class GetSiteByIdRequest:IRequest<Result<SiteResponse>>
    {
        public Guid Id { get; set; }
    }
}
