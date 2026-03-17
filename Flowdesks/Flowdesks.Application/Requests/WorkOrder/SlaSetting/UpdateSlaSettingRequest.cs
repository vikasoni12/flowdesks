using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder.SlaSetting
{
    public class UpdateSlaSettingListRequest : IRequest<Result<int>>
    {
        public List<UpdateSlaSettingRequest> Requests { get; set; }
    }

    public class UpdateSlaSettingRequest : CreateEditRequest<Domain.Entities.SystemPreferences.WorkOrder.SLASettings>, IRequest<Result<int>>
    {
        public Guid Id { get; set; }
        public string Color { get; set; }
        public int Minutes { get; set; }
    }
}