using Flowdesks.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Flowdesks.Domain.Entities.Chat
{
    public class Group : AuditableEntity<Guid>
    {
        [Required] public string GroupName { get; set; }
        public virtual ICollection<GroupMessage> GroupMessages { get; set; }
        public virtual ICollection<GroupUser> GroupUsers { get; set; }
    }
}
