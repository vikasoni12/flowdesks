using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Procedures.Command.Delete;

public class DeleteProcedureCommand : IRequest<Result<int>>
{
    public List<Guid>? Ids { get; set; }
}

public class DeleteProcedureCommandHandler : IRequestHandler<DeleteProcedureCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProcedureCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(DeleteProcedureCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var procedures = _unitOfWork.Repository<Procedure>().Entities().WhereIf(request.Ids != null && request.Ids.Count > 0, x => request.Ids.Contains(x.Id)).ToList();

            if (procedures != null)
            {
                _unitOfWork.Repository<Procedure>().DeleteRange(procedures);
                await _unitOfWork.SaveAsync(cancellationToken);
            }

            return await Result<int>.SuccessAsync("Procedure deleted successfully");
        }
        catch (Exception ex)
        {
            return await Result<int>.FailAsync(ex.Message);
        }
    }
}