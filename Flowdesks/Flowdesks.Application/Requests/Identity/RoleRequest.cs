using System.ComponentModel.DataAnnotations;

namespace Flowdesks.Application.Requests.Identity
{
    public class RoleRequest
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}