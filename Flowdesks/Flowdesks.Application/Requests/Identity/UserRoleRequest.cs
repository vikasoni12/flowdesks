using Flowdesks.Application.Requests.Teams;

namespace Flowdesks.Application.Requests.Identity
{
    public class UserRoleRequest
    {
        public Guid? Id { get; set; }
        public string RoleName { get; set; }
    }
}
