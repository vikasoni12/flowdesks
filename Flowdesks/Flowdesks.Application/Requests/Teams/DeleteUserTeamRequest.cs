namespace Flowdesks.Application.Requests.Teams
{
    public class DeleteUserTeamRequest
    {
        public Guid UserId { get; set; }
        public List<Guid>? TeamIds { get; set; }
    }
}
