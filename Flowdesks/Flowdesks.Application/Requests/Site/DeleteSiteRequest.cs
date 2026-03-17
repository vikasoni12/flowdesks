using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Site
{
    public class DeleteSiteRequest : IRequest<Result<List<Guid>>>
    {
        public List<Guid> Ids { get; set; }
    }
}
