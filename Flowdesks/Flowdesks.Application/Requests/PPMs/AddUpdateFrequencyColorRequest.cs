using Flowdesks.Domain.Entities.PPMs;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.PPMs;

public class AddUpdateFrequencyColorRequest : CreateEditRequest<PPMFrequencyColor>, IRequest<Result<int>>
{
    public string Title { get; set; }
    public string Color { get; set; }
}