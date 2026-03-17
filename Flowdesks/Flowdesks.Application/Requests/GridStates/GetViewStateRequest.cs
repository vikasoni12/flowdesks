using Flowdesks.Application.Responses.GridStates;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.GridStates
{
    public class GetViewStateRequest : IRequest<Result<ViewStateResponse>>
    {
        public EntityType EntityType { get; set; }
        public Guid UserId { get; set; }
    }
}
