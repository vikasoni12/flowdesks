using Flowdesks.Domain.Common;
using Flowdesks.Domain.Entities.Identity;

namespace Flowdesks.Domain.Entities.Chat
{
    public class GroupMessage : AuditableEntity<Guid>
    {
        public Guid GroupId { get; set; }
        public virtual Group Group { get; set; }

        public Guid SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        public Guid MessageId { get; set; }
        public virtual Message Message { get; set; }

        public Guid? AttachmentId { get; set; }
        public MessageAttachment Attachment { get; set; }
    }
}
