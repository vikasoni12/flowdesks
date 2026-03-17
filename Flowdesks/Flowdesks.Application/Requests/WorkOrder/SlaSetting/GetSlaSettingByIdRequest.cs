using Flowdesks.Application.Responses.WorkOrder.SlaSetting;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder.SlaSetting
{
    public class GetSlaSettingByIdRequest : IRequest<Result<SlaSettingResponse>>
    {
        public Guid Id { get; set; }
    }
}
