using Flowdesks.Application.Responses.Chat.DirectMessage;

namespace Flowdesks.Application.Requests.Chat.GroupMessage
{
    public class AddUpdateGroupMessageRequest
    {
        public string Content { get; set; }
        public Guid GroupId { get; set; }
        public AttachmentDto? Attachment { get; set; }
    }
}
