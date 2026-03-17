using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.ProcedureMappings.Command.Delete;

public class DeleteProcedureMappingCommand : IRequest<Result<int>>
{
    public List<Guid> Ids { get; set; }
}

public class DeleteProcedureMappingCommandHandler : IRequestHandler<DeleteProcedureMappingCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProcedureMappingCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(DeleteProcedureMappingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var procedureMappingIds = request.Ids ?? new List<Guid>();

            var procedures = _unitOfWork.Repository<ProcedureMapping>().Entities()
                .Include(x => x.Responses)
                .ThenInclude(x => x.Attachment)
                .WhereIf(procedureMappingIds.Any(), x => procedureMappingIds.Contains(x.Id))
                .ToList();

            var attachmentIdsToDelete = procedures
                .SelectMany(pm => pm.Responses)
                .Where(r => r.AttachmentId != null)
                .Select(r => r.AttachmentId.Value)
                .ToList();

            var attachments = _unitOfWork.Repository<MessageAttachment>().Entities()
                .Where(a => attachmentIdsToDelete.Contains(a.Id))
                .ToList();

            if (attachments != null) _unitOfWork.Repository<MessageAttachment>().DeleteRange(attachments, true);

            _unitOfWork.Repository<ProcedureMapping>().DeleteRange(procedures);
            await _unitOfWork.SaveAsync(cancellationToken);

            return await Result<int>.SuccessAsync("Procedure deleted successfully");
        }
        catch (Exception ex)
        {
            return await Result<int>.FailAsync(ex.Message);
        }
    }
}