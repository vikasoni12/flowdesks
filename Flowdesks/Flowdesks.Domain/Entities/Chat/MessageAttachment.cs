using Flowdesks.Domain.Common;

namespace Flowdesks.Domain.Entities.Chat
{
    public class MessageAttachment : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }

        public virtual DirectMessage DirectMessage { get; set; }
        public virtual GroupMessage GroupMessage { get; set; }
    }
}
