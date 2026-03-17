using Flowdesks.Application.Requests;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Contacts;

namespace Flowdesks.Application.Specifications.Contacts
{
    public class ContactClassFilterSpecification : Specification<ContactClass>
    {
        public ContactClassFilterSpecification(PagedRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch));
            }
        }
    }
}
