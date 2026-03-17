using Flowdesks.Shared.Enums;

namespace Flowdesks.Application.Responses.GridStates
{
    public class ViewStateResponse
    {
        public Guid Id { get; set; }
        public EntityType EntityType { get; set; }
        public ViewType ViewType { get; set; }
        public Guid UserId { get; set; }
    }
}