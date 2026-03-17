using Flowdesks.Application.Responses.Chat.DirectMessage;

namespace Flowdesks.Application.Responses.Procedures;

public class ProcedureResponse
{
    public Guid Id { get; set; }
    public Guid ProcedureQuestionId { get; set; }
    public Guid ProcedureMappingResponseId { get; set; }
    public string Response { get; set; }
    public AttachmentDto? Attachment { get; set; }
}
