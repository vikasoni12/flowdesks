using Flowdesks.Application.Attributes;
using Flowdesks.Domain.Entities.Contacts;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.Contacts;

public class AddContactRequest : CreateEditRequest<Contact>, IRequest<Result<int>>
{
    [IsRequiredField(true)]
    [Description("Full Name")]
    public string FullName { get; set; }
    [IsRequiredField(true)]
    public string Email { get; set; }
    [Description("Contact Class")]
    public Guid? ContactClassId { get; set; }
    [IsRequiredField(true)]
    [Description("Phone Number")]
    public string? PhoneNumber { get; set; }
    [IgnoreField(true)]
    public Guid EntityId { get; set; }
    [IgnoreField(true)]
    public EntityType EntityType { get; set; }
}