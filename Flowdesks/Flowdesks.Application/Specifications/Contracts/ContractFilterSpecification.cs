using Flowdesks.Application.Requests.Contract;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.Contracts
{
    public class ContractFilterSpecification : Specification<Domain.Entities.Contracts.Contract>
    {
        public ContractFilterSpecification(ContractPagingRequest request)
        {
            if (request.SupplierId != null)
            {
                And(p => p.SupplierId != null && request.SupplierId == p.SupplierId);
            }
        }
    }
}
