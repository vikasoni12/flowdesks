using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using PermitEntity = Flowdesks.Domain.Entities.Permit.Permit;

namespace Flowdesks.Application.Features.Permit.Command.Delete;

public class DeletePermitCommand : IRequest<Result<int>>
{
    public List<Guid>? Ids { get; set; }
}

public class DeletePermitCommandHandler : IRequestHandler<DeletePermitCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePermitCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(DeletePermitCommand request, CancellationToken cancellationToken)
    {
        var permits = _unitOfWork.Repository<PermitEntity>().Entities()
          .WhereIf(request.Ids != null && request.Ids.Count > 0, x => request.Ids.Contains(x.Id)).ToList();

        if (permits == null || permits.Count <= 0)
        {
            return await Result<int>.FailAsync($"Not found");
        }

        _unitOfWork.Repository<PermitEntity>().DeleteRange(permits, true);

        await _unitOfWork.SaveAsync(cancellationToken);

        return await Result<int>.SuccessAsync("Permit deleted successfully");
    }
}