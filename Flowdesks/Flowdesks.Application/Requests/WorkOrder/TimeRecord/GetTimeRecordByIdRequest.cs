using Flowdesks.Application.Responses.WorkOrder.TimeRecord;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder.TimeRecord
{
    public class GetTimeRecordByIdRequest : IRequest<Result<TimeRecordResponse>>
    {
        public Guid Id { get; set; }
    }
}

