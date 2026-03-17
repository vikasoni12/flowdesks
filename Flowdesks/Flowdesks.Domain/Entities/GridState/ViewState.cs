using Flowdesks.Domain.Common;
using Flowdesks.Domain.Entities.Identity;

namespace Flowdesks.Domain.Entities.GridState
{
    public class ViewState : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string EntityType { get; set; }
        public string ViewType { get; set; }
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
