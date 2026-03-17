using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder.TimeRecord
{
    public class UpdateTimeRecordRequest : CreateEditRequest<Domain.Entities.WorkOrder.TimeRecord.TimeRecord>, IRequest<Result<int>>
    {
        public Guid Id { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? FinishDateTime { get; set; }
        public Guid? TechnicianId { get; set; }
        public Guid? TaskId { get; set; }
    }
}
