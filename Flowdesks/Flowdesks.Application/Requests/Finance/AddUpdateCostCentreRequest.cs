using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Finance
{
    public class AddUpdateCostCentreRequest : IRequest<Result<int>>
    {
        public string Name { get; set; }
        public decimal Budget {  get; set; }
    }
}
