using Flowdesks.Application.Responses.Chat.DirectMessage;
using Flowdesks.Application.Responses.Chat.Groups;

namespace Flowdesks.Application.Responses.Chat.GroupMessage
{
    public class GroupMessageResponse
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public UserDirectMessagesResponse Sender { get; set; }
        public GroupResponse Group { get; set; }
        public AttachmentDto Attachment { get; set; }
    }
}
