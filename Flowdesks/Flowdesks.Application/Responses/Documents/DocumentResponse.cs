using Flowdesks.Shared.Enums;

namespace Flowdesks.Application.Responses.Documents
{
    public class DocumentResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ExternalUrl { get; set; }
        public string? DocumentType { get; set; }
        public Guid EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public DateTime DocumentDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

        public List<DocumentFileResponse> Files { get; set; }
    }
}