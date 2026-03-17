namespace Flowdesks.Application.Requests.Teams;

public class TeamUserRequest
{
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
}
