using Flowdesks.Application.Responses.Chat.DirectMessage;

namespace Flowdesks.Application.Requests.Chat.DirectMessage
{
    public class AddUpdateDirectMessageRequest
    {
        public string Content { get; set; }
        public Guid ReceiverId { get; set; }
        public AttachmentDto? Attachment { get; set; }
    }
}
