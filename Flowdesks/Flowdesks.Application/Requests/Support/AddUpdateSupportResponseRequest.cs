using Flowdesks.Application.Responses.Chat.DirectMessage;

namespace Flowdesks.Application.Requests.Support;

public class AddUpdateSupportResponseRequest
{
    public string Response { get; set; }
    public Guid? AttachmentId { get; set; }
    public AttachmentDto? Attachment { get; set; }
    public Guid SupportId { get; set; }
    public Guid? RespondedBy { get; set; }
}
