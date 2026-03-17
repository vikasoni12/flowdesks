using Flowdesks.Domain.Common;

namespace Flowdesks.Domain.Entities.Documents;

public class Document : AuditableEntity<Guid>
{
    public string Title { get; set; }
    public string? ExternalUrl {  get; set; }
    public string? DocumentType { get; set; }
    public DateTime? DocumentDate { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } //EntityType Enum

    public virtual ICollection<DocumentFile> Files { get; set; }
}