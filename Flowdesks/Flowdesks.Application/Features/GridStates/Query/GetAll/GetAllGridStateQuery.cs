using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.GridStates;
using Flowdesks.Application.Responses.GridStates;
using Flowdesks.Application.Specifications.GridStates;
using Flowdesks.Domain.Entities.GridState;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Newtonsoft.Json;

namespace Flowdesks.Application.Features.GridStates.Query.GetAll;

public class GetAllGridStateQuery : GridStateGetAllRequest, IRequest<Result<GridStateResponse>>
{
}
internal class GetAllGridStateQueryHandler : IRequestHandler<GetAllGridStateQuery, Result<GridStateResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetAllGridStateQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GridStateResponse>> Handle(GetAllGridStateQuery request, CancellationToken cancellationToken)
    {
        try
        {
            GridStateSpecificationFilter spec = new(request);

            var userId = _currentUserService.UserId;

            var gridState = _unitOfWork.Repository<GridState>().Entities()
                .Specify(spec).FirstOrDefault(x => x.CreatedBy == userId);

            if (gridState == null)
            {
                return Result<GridStateResponse>.Success("No data found");
            }

            var gridColumns = (JsonConvert.DeserializeObject<List<ColumnRequest>>(gridState.Data)).OrderBy(x => x.Order).ToList();

            var gridStateResponse = _mapper.Map<GridStateResponse>(gridState);
            gridStateResponse.Columns = gridColumns;
            return Result<GridStateResponse>.Success(gridStateResponse);

        }
        catch (Exception ex)
        {
            return Result<GridStateResponse>.Fail(ex.Message);
        }
    }
}
