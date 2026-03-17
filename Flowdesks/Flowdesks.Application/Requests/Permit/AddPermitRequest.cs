using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Permit;

public class AddPermitRequest : CreateEditRequest<Domain.Entities.Permit.Permit>, IRequest<Result<int>>
{
    public string Name { get; set; }
    public string Description { get; set; }
}