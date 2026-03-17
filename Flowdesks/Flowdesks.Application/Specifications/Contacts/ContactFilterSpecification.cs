using Flowdesks.Application.Requests.Contacts;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Contacts;
using Flowdesks.Shared.Enums;

namespace Flowdesks.Application.Specifications.Contacts;

public class ContactFilterSpecification : Specification<Contact>
{
    public ContactFilterSpecification(ContactPagingRequest request)
    {
        if (!string.IsNullOrEmpty(Enum.GetName(typeof(EntityType), request.EntityType)))
        {
            var value = Enum.GetName(typeof(EntityType), request.EntityType);
            And(p => p.EntityType.Equals(value));
        }

        if (!string.IsNullOrEmpty(request.EntityId))
        {
            And(p => p.EntityId.ToString().Equals(request.EntityId));
        }

        if (!string.IsNullOrEmpty(request.CreatedBy))
        {
            And(p => p.CreatedBy.Equals(request.CreatedBy));
        }

        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.FullName.Contains(request.StringSearch) ||
            p.Email.Contains(request.StringSearch) ||
            p.ContactClass.Name.Contains(request.StringSearch) ||
            p.PhoneNumber.Contains(request.StringSearch)
            );
        }
    }

    public ContactFilterSpecification(AddContactRequest request)
    {
        if (!string.IsNullOrEmpty(Enum.GetName(typeof(EntityType), request.EntityType)))
        {
            var value = Enum.GetName(typeof(EntityType), request.EntityType);
            And(p => p.EntityType.Equals(value));
        }      
    }

    public ContactFilterSpecification(UpdateContactRequest request)
    {
        var value = Enum.GetName(typeof(EntityType), request.EntityType);
        if (!string.IsNullOrEmpty(value))
        {
            And(p => p.EntityType.Equals(value));
        }       
    }
}
