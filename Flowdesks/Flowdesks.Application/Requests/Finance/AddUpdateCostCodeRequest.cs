using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Finance
{
    public class AddUpdateCostCodeRequest : IRequest<Result<int>>
    {
        public string Name { get; set; }
    }
}
