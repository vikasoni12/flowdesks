using Flowdesks.Domain.Entities.Sites;
using System.ComponentModel.DataAnnotations;

namespace Flowdesks.Domain.MasterEntities
{
    public class Country
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }     
        public virtual ICollection<Site> Sites { get; set; }
    }
}
 