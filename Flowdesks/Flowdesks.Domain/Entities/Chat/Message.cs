using Flowdesks.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Flowdesks.Domain.Entities.Chat
{
    public class Message : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        [Required] public string Content { get; set; }
    }
}
