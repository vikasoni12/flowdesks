using Flowdesks.Application.Responses.Contract;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Contract
{
    public class GetContractByIdRequest : IRequest<Result<ContractResponse>>
    {
        public Guid ContractId { get; set; }
    }
}
