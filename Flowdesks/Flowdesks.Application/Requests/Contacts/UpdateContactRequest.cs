using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Contacts;

public class UpdateContactRequest : IRequest<Result<int>>
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public Guid? ContactClassId { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid EntityId { get; set; }
    public EntityType EntityType { get; set; }
}
