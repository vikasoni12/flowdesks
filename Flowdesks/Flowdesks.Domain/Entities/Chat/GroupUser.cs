using Flowdesks.Domain.Common;
using Flowdesks.Domain.Entities.Identity;

namespace Flowdesks.Domain.Entities.Chat
{
    public class GroupUser : AuditableEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public DateTime? LastDeletedOn { get; set; }
        public DateTime? LastReadOn { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }
    }
}
