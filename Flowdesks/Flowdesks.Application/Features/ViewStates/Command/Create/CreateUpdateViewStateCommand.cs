using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.GridStates;
using Flowdesks.Domain.Entities.GridState;
using Flowdesks.Domain.Entities.Teams;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.ViewStates.Command.Create
{
    public class CreateUpdateViewStateCommand : CreateUpdateViewStateRequest, IRequest<Result<int>>
    { }

    public class CreateViewStateCommandHandler : IRequestHandler<CreateUpdateViewStateCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateViewStateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateUpdateViewStateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var alreadyExists = _unitOfWork.Repository<ViewState>().Entities()
                    .FirstOrDefault(x => x.EntityType.Equals(request.EntityType.ToString(), StringComparison.OrdinalIgnoreCase)
                    && x.UserId == request.UserId);

                if (alreadyExists != null)
                {
                    if (request.ViewType == Shared.Enums.ViewType.Grid)
                    {
                        _unitOfWork.Repository<ViewState>().Delete(alreadyExists.Id);
                    }
                    else
                    {
                        _unitOfWork.Repository<ViewState>().Update(_mapper.Map(request, alreadyExists));
                    }
                }
                else
                {
                    if (request.ViewType != Shared.Enums.ViewType.Grid)
                        _unitOfWork.Repository<ViewState>().Add(_mapper.Map<ViewState>(request));
                }

                await _unitOfWork.SaveAsync(cancellationToken);

                return Result<int>.Success("ViewState saved successfully");
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex.Message);
            }
        }
    }
}
