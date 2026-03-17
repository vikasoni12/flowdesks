using Flowdesks.Application.Responses.Teams;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Teams;

public class GetTeamByIdRequest : IRequest<Result<TeamResponse>>
{
    public Guid Id { get; set; }
}
