using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Chats.Groups.Command;

public class DeleteGroupCommand : IRequest<Result<int>>
{
    public Guid Id { get; set; }
}

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUploadService _uploadService;

    public DeleteGroupCommandHandler(IUnitOfWork unitOfWork, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork;
        _uploadService = uploadService;
    }

    public async Task<Result<int>> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var group = await _unitOfWork.Repository<Group>().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (group == null)
            {
                return await Result<int>.FailAsync("Invalid group id");
            }

            var messages = _unitOfWork.Repository<GroupMessage>().Entities()
                .Include(x => x.Message).Include(x => x.Attachment)
                .Where(x => x.GroupId.Equals(request.Id));

            if (messages.Any())
            {
                var attachments = messages.Where(x => x.Attachment != null)
                           .Select(x => x.Attachment);
                if (attachments.Any())
                {
                    await _uploadService.DeleteManyAsync(attachments.Select(x => x.Url).ToList());
                    _unitOfWork.Repository<MessageAttachment>().DeleteRange(attachments, true);
                }

                _unitOfWork.Repository<Message>().DeleteRange(messages.Select(x => x.Message), true);
            }

            _unitOfWork.Repository<Group>().Delete(request.Id);

            await _unitOfWork.SaveAsync(cancellationToken);

            return await Result<int>.SuccessAsync("Group deleted successfully");
        }
        catch (Exception ex)
        {
            return await Result<int>.FailAsync(ex.Message);
        }
    }
}
