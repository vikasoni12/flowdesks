using Flowdesks.Application.Responses.Chat.DirectMessage;

namespace Flowdesks.Application.Requests.Procedures;

public class ProcedureResponseRequest
{
    public Guid? Id { get; set; }
    public Guid ProcedureQuestionId { get; set; }
    public Guid ProcedureMappingId { get; set; }
    public Guid? AttachmentId { get; set; }
    public string Response { get; set; }
    public UploadByteArray? Image { get; set; } = new();
    public AttachmentDto? Attachment { get; set; }
}