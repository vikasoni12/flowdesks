using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Asset
{
    public class UpdateAssetHeirarchyRequest : IRequest<Result<int>>
    {
        public Guid Id { get; set; }
        public List<Guid> ChildAssets { get; set; }
    }
}
