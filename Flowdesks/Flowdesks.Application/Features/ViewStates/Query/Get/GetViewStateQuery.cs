using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.GridStates;
using Flowdesks.Application.Responses.GridStates;
using Flowdesks.Domain.Entities.GridState;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Teams.Query.GetById;

public class GetViewStateQuery : IRequestHandler<GetViewStateRequest, Result<ViewStateResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetViewStateQuery(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ViewStateResponse>> Handle(GetViewStateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var view = await _unitOfWork.Repository<ViewState>().Entities()
                .FirstOrDefaultAsync(x => x.EntityType.Equals(request.EntityType.ToString(), StringComparison.OrdinalIgnoreCase)
                    && x.UserId == request.UserId, cancellationToken: cancellationToken);

            if (view == null)
            {
                var viewState = new ViewStateResponse()
                {
                    UserId = request.UserId,
                    EntityType = request.EntityType,
                    ViewType = Shared.Enums.ViewType.Grid
                };
                return await Result<ViewStateResponse>.SuccessAsync(viewState);
            }

            var response = _mapper.Map<ViewStateResponse>(view);

            return await Result<ViewStateResponse>.SuccessAsync(response);
        }
        catch (Exception ex)
        {
            return Result<ViewStateResponse>.Fail(ex.Message);
        }
    }
}