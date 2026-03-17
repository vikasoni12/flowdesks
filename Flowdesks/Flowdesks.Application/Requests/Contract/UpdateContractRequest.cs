using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Contract;

public class UpdateContractRequest : CreateEditRequest<Domain.Entities.Contracts.Contract>, IRequest<Result<int>>
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public Guid? SupplierId { get; set; }
}
