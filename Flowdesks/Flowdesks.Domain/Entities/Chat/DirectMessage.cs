using Flowdesks.Domain.Common;
using Flowdesks.Domain.Entities.Identity;

namespace Flowdesks.Domain.Entities.Chat
{
    public class DirectMessage : FullAuditableEntity<Guid>
    {
        public bool IsRead { get; set; }

        public Guid MessageId { get; set; }
        public virtual Message Message { get; set; } = new Message();

        public Guid ReceiverId { get; set; }
        public virtual ApplicationUser Receiver { get; set; }

        public Guid SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }

        public Guid? AttachmentId { get; set; }
        public MessageAttachment Attachment { get; set; }
    }
}
