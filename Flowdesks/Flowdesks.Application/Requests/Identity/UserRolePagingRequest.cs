namespace Flowdesks.Application.Requests.Identity
{
    public class UserRolePagingRequest : PagedRequest
    {
        public Guid UserId { get; set; }
    }
}
