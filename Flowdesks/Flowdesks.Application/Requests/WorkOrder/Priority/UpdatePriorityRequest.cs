using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder.Priority
{
    public class UpdatePriorityRequest : CreateEditRequest<Domain.Entities.SystemPreferences.WorkOrder.Priority>, IRequest<Result<int>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Minutes { get; set; }
        public int RankOrder { get; set; }
    }
}
