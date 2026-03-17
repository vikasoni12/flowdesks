using Flowdesks.Shared.Enums;

namespace Flowdesks.Application.Requests.RequiredFields
{
    public class RequiredFieldsPagingRequest : PagedRequest
    {
        public bool IsFromGrid { get; set; } = false;
        public EntityType EntityType { get; set; }
    }
}
