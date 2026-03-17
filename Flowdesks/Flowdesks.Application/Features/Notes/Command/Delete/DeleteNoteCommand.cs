using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Domain.Entities.Note;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Notes.Command.Delete;

public class DeleteNoteCommand : IRequest<Result<int>>
{
    public Guid Id { get; set; }
}

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNoteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Note note = await _unitOfWork.Repository<Note>().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (note != null)
            {
                _unitOfWork.Repository<Note>().Delete(request.Id);
                await _unitOfWork.SaveAsync(cancellationToken);
            }

            return await Result<int>.SuccessAsync("Note deleted successfully");
        }
        catch (Exception ex)
        {
            return await Result<int>.FailAsync(ex.Message);
        }
    }
}