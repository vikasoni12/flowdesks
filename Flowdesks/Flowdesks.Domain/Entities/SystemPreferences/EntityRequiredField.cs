using Flowdesks.Domain.Common;

namespace Flowdesks.Domain.Entities.SystemPreferences
{
    public class EntityRequiredField : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Value { get; set; }
        public string Title { get; set; }
        public string EntityType { get; set; }

        public EntityRequiredField() { }

        public EntityRequiredField(Guid tenantId, string value, string title, string entityType)
        {
            TenantId = tenantId;
            Value = value;
            Title = title;
            EntityType = entityType;
        }
    }
}
