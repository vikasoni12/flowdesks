using Flowdesks.Application.Requests.PPMs;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.PPMs;

namespace Flowdesks.Application.Specifications.PPMs
{
    public class FrequencyColorFilterSpecification : Specification<PPMFrequencyColor>
    {
        public FrequencyColorFilterSpecification(FrequencyColorPagingRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Title.Contains(request.StringSearch));
            }
        }
    }
}
