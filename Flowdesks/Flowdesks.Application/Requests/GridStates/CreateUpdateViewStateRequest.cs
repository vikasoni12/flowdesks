using Flowdesks.Shared.Enums;

namespace Flowdesks.Application.Requests.GridStates
{
    public class CreateUpdateViewStateRequest
    {
        public EntityType EntityType { get; set; }
        public ViewType ViewType { get; set; }
        public Guid UserId { get; set; }
    }
}
