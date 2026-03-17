using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Interfaces.Site
{
    public interface ISiteRepository
    {
        Task<Domain.Entities.Sites.Site> GetSiteById(Guid siteId);
        Task<List<Domain.Entities.Sites.Site>> GetAllSite();

    }
}
