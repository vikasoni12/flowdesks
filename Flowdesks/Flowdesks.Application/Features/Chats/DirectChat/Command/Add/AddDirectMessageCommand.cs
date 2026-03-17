using AutoMapper;
using Flowdesks.Application.Interfaces.Identity;
using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Chat.DirectMessage;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Application.Responses.Chat.DirectMessage;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Shared.Constants.Chats;
using Flowdesks.Shared.Notification;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Chats.DirectChat.Command;

public class AddDirectMessageCommand : AddUpdateDirectMessageRequest, IRequest<Result<DirectMessageResponse>>
{

}

public class AddDirectMessageCommandHandler : IRequestHandler<AddDirectMessageCommand, Result<DirectMessageResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;
    private readonly IUserService _userService;

    public AddDirectMessageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, INotificationService notificationService, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _notificationService=notificationService;
        _userService=userService;
    }

    public async Task<Result<DirectMessageResponse>> Handle(AddDirectMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var message = _mapper.Map<DirectMessage>(request);

            message.SenderId = new Guid(_currentUserService.UserId);

            var response = _unitOfWork.Repository<DirectMessage>().Add(message);

            NotificationSetting notificationSetting = await _unitOfWork.Repository<NotificationSetting>().Entities()
                                                            .FirstOrDefaultAsync(x => x.UserId == request.ReceiverId &&
                                                             (x.MessageType == ChatMessageConstant.DirectMessage ||
                                                             x.MessageType == ChatMessageConstant.Both), cancellationToken: cancellationToken);

            if (notificationSetting != null)
            {
                var user = (await _userService.GetAsync(message.SenderId)).Data;
                await CreateNotification( $"{user.FirstName} {user.LastName}",
                    request.Content.Length >100? $"{request.Content.Substring(0,100)}...":request.Content,
                    message.SenderId.ToString(), notificationSetting.UserId.ToString());
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            var result = await _unitOfWork.Repository<DirectMessage>().Entities()
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .FirstOrDefaultAsync(x => x.Id.Equals(response.Id), cancellationToken: cancellationToken);

            return Result<DirectMessageResponse>.Success(_mapper.Map<DirectMessageResponse>(result));
        }
        catch (Exception ex)
        {
            return Result<DirectMessageResponse>.Fail(ex.Message);
        }
    }

    private async Task CreateNotification(string content, string message,string userId, string entityId)
    {
        await _notificationService.Create(new CreateUpdateNotificationRequest
        {
            Content = content,
            Message = message,
            LinkToNotification = $"/chat/user/{userId}",
            EntityId = entityId,
            NotificationType = NotificationType.Chat
        });
    }
}
