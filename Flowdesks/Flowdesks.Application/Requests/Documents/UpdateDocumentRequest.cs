using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Documents;

public class UpdateDocumentRequest : IRequest<Result<int>>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string DocumentType { get; set; }
    public DateTime? DocumentDate { get; set; }
    public string ExternalUrl { get; set; }
    public List<DocumentFileRequest> Files { get; set; }
}
