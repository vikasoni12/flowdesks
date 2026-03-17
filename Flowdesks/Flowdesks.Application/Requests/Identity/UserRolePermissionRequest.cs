namespace Flowdesks.Application.Requests.Identity
{
    public class UserRolePermissionRequest
    {
        public Guid? UserId { get; set; }
        public List<Guid>? RoleIds { get; set; }
    }
}