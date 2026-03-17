using Flowdesks.Domain.Entities.Chat;

namespace Flowdesks.Application.Responses.Chat.DirectMessage
{
    public class DirectMessageResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public UserDirectMessagesResponse Sender { get; set; }
        public UserDirectMessagesResponse Receiver { get; set; }
        public AttachmentDto Attachment { get; set; }
    }
}
