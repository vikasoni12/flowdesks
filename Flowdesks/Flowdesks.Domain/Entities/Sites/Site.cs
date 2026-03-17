using Flowdesks.Domain.Common;
using Flowdesks.Domain.Entities.Identity;
using Flowdesks.Domain.MasterEntities;

namespace Flowdesks.Domain.Entities.Sites
{
    public class Site : AuditableEntity<Guid>
    {
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PostCode { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? ContactNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public Guid? CountryId { get; set; }
        public virtual Country SiteCountry { get; set; }
        public virtual ICollection<ApplicationUser>? Users { get; set; }
    }
}