using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.GridStates;
using Flowdesks.Domain.Entities.GridState;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Newtonsoft.Json;

namespace Flowdesks.Application.Features.GridStates.Command.Create;

public class CreateGridStateCommand : IRequestHandler<GridStateRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateGridStateCommand(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<Result<int>> Handle(GridStateRequest request, CancellationToken cancellationToken)
    {
        try
        {

            var userId = _currentUserService.UserId;

            var existingGridState = _unitOfWork.Repository<GridState>().Entities()
                                                  .FirstOrDefault(x => x.TableName == request.TableName.ToString() && x.CreatedBy == userId);
            if (existingGridState != null)
            {
                _unitOfWork.Repository<GridState>().Delete(existingGridState.Id);
            }
            var data = JsonConvert.SerializeObject(request.Columns);
            GridState gridState = new()
            {
                Data = data,
                TableName = request.TableName.ToString(),
                PageSize = request.PageSize,
            };

            _unitOfWork.Repository<GridState>().Add(gridState);
            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<int>.Success("Grid State saved successfully");

        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}
