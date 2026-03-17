using Flowdesks.Application.Requests.WorkOrder.SlaSetting;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.WorkOrder
{
    public class SlaSettingFilterSpecification : Specification<Domain.Entities.SystemPreferences.WorkOrder.SLASettings>
    {
        public SlaSettingFilterSpecification(SlaSettingPagingRequest request) { }
    }
}
