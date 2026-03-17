using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace Flowdesks.Application.Features.Chats.DirectChat.Command;

public class DeleteDirectMessageCommand : IRequest<Result<int>>
{
    public Guid Id { get; set; }
}

public class DeleteDirectMessageCommandHandler : IRequestHandler<DeleteDirectMessageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUploadService _uploadService;

    public DeleteDirectMessageCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _uploadService = uploadService;
    }

    public async Task<Result<int>> Handle(DeleteDirectMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var message = await _unitOfWork.Repository<DirectMessage>().Entities()
                .Include(x => x.Attachment)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (message == null)
            {
                return await Result<int>.FailAsync("Message not found");
            }

            if (message.SenderId.ToString() != _currentUserService.UserId)
            {
                return await Result<int>.FailAsync("Authorization failed");
            }

            if (message.Attachment != null)
            {
                await _uploadService.DeleteAsync(message.Attachment.Url);

                _unitOfWork.Repository<MessageAttachment>().Delete(message.Attachment.Id, true);
            }

            _unitOfWork.Repository<Message>().Delete(message.MessageId, true);
            await _unitOfWork.SaveAsync(cancellationToken);

            return await Result<int>.SuccessAsync("Message deleted successfully");
        }
        catch (Exception ex)
        {
            return await Result<int>.FailAsync(ex.Message);
        }
    }
}
