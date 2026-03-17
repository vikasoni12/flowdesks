using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Identity;

namespace Flowdesks.Infrastructure.Specifications
{
    public class RoleFilterSpecification : Specification<Role>
    {
        public RoleFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
