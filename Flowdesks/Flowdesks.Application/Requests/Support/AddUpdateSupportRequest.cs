using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Support;

public class AddUpdateSupportRequest : CreateEditRequest<Domain.Entities.Support.Support>, IRequest<Result<int>>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string DocumentUrl { get; set; }
    public string Status {  get; set; }
    public Guid? PriorityId {  get; set; }
    public Guid? AssignedUserId { get; set; }
    public Guid? UserId { get; set; }
    public UploadByteArray? Document { get; set; } = new();
}