using Flowdesks.Application.Attributes;
using Flowdesks.Domain.Entities.Documents;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.Documents;

public class AddDocumentRequest : CreateEditRequest<Document>, IRequest<Result<int>>
{
    [IsRequiredField(true)]
    public string Title { get; set; }
    [Description("Document type")]
    public string DocumentType { get; set; }
    [IgnoreField(true)]
    public string ExternalUrl { get; set; }

    [Description("Date")]
    public DateTime? DocumentDate { get; set; }

    [IgnoreField(true)]
    public Guid EntityId { get; set; }

    [IgnoreField(true)]
    public EntityType EntityType { get; set; }

    [IgnoreField(true)]
    public List<DocumentFileRequest> Files { get; set; }
}