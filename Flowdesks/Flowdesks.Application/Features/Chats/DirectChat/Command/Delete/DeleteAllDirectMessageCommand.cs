using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Specifications.Chat;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Chats.DirectChat.Command;

public class DeleteAllDirectMessageCommand : IRequest<Result<int>>
{
    public Guid UserId { get; set; }
}

public class DeleteAllDirectMessageCommandHandler : IRequestHandler<DeleteAllDirectMessageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUploadService _uploadService;

    public DeleteAllDirectMessageCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _uploadService = uploadService;
    }

    public async Task<Result<int>> Handle(DeleteAllDirectMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = _currentUserService.UserId;

            DirectMessageFilterSpecification spec = new(request, currentUserId);

            var query = _unitOfWork.Repository<DirectMessage>().Entities().Include(x => x.Message)
                .Include(x => x.Attachment).Specify(spec);

            var deleteForBoth = query.Where(x => x.IsDeleted && x.DeletedBy != currentUserId);

            if (deleteForBoth.Any())
            {
                //delete files if exists

                var attachments = deleteForBoth.Select(x => x.Attachment);

                await _uploadService.DeleteManyAsync(attachments.Select(x => x.Url).ToList());

                _unitOfWork.Repository<MessageAttachment>().DeleteRange(attachments, true);

                _unitOfWork.Repository<Message>().DeleteRange(deleteForBoth.Select(x => x.Message), true);
            }

            var deleteForCurrentUser = query.Where(x => !x.IsDeleted);

            if (deleteForCurrentUser.Any())
                _unitOfWork.Repository<DirectMessage>().DeleteRange(deleteForCurrentUser, false);

            await _unitOfWork.SaveAsync(cancellationToken);

            return await Result<int>.SuccessAsync("Deleted successfully");
        }
        catch (Exception ex)
        {
            return await Result<int>.FailAsync(ex.Message);
        }
    }
}
