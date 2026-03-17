using Flowdesks.Domain.Common;
using Flowdesks.Domain.Entities.Sites;

namespace Flowdesks.Domain.Entities.Identity
{
    public class UserSite : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid UserId { get; set; }
        public Guid SiteId { get; set; }

        public Site Site { get; set; }
        public ApplicationUser User { get; set; }
    }
}
