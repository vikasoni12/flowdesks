using Flowdesks.Domain.Entities.PPMs;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.PPMs;

public class AddUpdatePPMReminderSettingsRequest : CreateEditRequest<PPMReminderSettings>, IRequest<Result<int>>
{
    public int? TriggerDays { get; set; }
}