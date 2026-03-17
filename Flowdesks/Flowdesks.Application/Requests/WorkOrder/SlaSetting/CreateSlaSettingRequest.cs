using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder.SlaSetting
{
    public class CreateSlaSettingRequest : CreateEditRequest<Domain.Entities.SystemPreferences.WorkOrder.SLASettings>, IRequest<Result<int>>
    {
        public string Color { get; set; }
        public int Minutes { get; set; }
    }
}
