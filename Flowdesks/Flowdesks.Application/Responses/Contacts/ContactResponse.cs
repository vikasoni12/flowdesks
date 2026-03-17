using Flowdesks.Shared.Enums;

namespace Flowdesks.Application.Responses.Contacts;

public class ContactResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string ContactClass { get; set; }
    public Guid? ContactClassId { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid EntityId { get; set; }
    public EntityType EntityType { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}