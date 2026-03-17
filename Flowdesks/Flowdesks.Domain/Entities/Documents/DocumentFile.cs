using Flowdesks.Domain.Common;

namespace Flowdesks.Domain.Entities.Documents
{
    public class DocumentFile : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Url { get; set; }
        public Guid DocumentId { get; set; }
        public Document Document { get; set; }
    }
}
