using AutoMapper;
using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Chat.GroupMessage;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Application.Responses.Chat.GroupMessage;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Shared.Constants.Chats;
using Flowdesks.Shared.Notification;
using Flowdesks.Shared.Wrapper;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Flowdesks.Application.Features.Chats.GroupChat.Command;

public class AddGroupMessageCommand : AddUpdateGroupMessageRequest, IRequest<Result<GroupMessageResponse>>
{

}

public class AddGroupMessageCommandHandler : IRequestHandler<AddGroupMessageCommand, Result<GroupMessageResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;

    public AddGroupMessageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _notificationService=notificationService;
    }

    public async Task<Result<GroupMessageResponse>> Handle(AddGroupMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var group = await _unitOfWork.Repository<Group>().Entities().Include(x => x.GroupUsers)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.GroupId), cancellationToken: cancellationToken);

            if (group == null)
            {
                return Result<GroupMessageResponse>.Fail("Invalid group id");
            }

            var message = _mapper.Map<GroupMessage>(request);
            message.SenderId = new Guid(_currentUserService.UserId);

            var addedMessage = _unitOfWork.Repository<GroupMessage>().Add(message);

            List<Guid> groupUserIds =  group.GroupUsers.Select(user => user.UserId).Where(x=>x != message.SenderId).ToList();

            List<NotificationSetting> notificationSettings = await _unitOfWork.Repository<NotificationSetting>().Entities()
                                    .Where(x => groupUserIds.Contains((Guid)x.UserId) &&
                                     (x.MessageType == ChatMessageConstant.Channels ||
                                     x.MessageType == ChatMessageConstant.Both)).ToListAsync();

            if (notificationSettings.Any())
            {
                foreach (var userNotificationSetting in notificationSettings)
                {
                    if (userNotificationSetting != null)
                    {
                        await CreateNotification($"{group.GroupName}",
                            request.Content.Length >100 ? $"{request.Content.Substring(0, 100)}..." : request.Content,
                            group.Id.ToString(), userNotificationSetting.UserId.ToString());
                    }
                }
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            var result = await _unitOfWork.Repository<GroupMessage>().Entities().Include(x => x.Sender).FirstOrDefaultAsync(x => x.Id.Equals(addedMessage.Id));

            BackgroundJob.Enqueue(() => UpdateLastRead(_currentUserService.UserId, message.GroupId));

            return Result<GroupMessageResponse>.Success(_mapper.Map<GroupMessageResponse>(result));
        }
        catch (Exception ex)
        {
            return Result<GroupMessageResponse>.Fail(ex.Message);
        }
    }

    public async Task UpdateLastRead(string currentUserId, Guid groupId)
    {
        var currentUser = await _unitOfWork.Repository<GroupUser>().Entities()
            .FirstOrDefaultAsync(x => x.GroupId.Equals(groupId) && x.UserId.ToString().Equals(currentUserId));

        if (currentUser != null)
        {
            currentUser.LastReadOn = DateTime.UtcNow;

            _unitOfWork.Repository<GroupUser>().Update(currentUser);

            await _unitOfWork.SaveAsync();
        }
    }

    private async Task CreateNotification(string content, string message, string groupId, string entityId)
    {
        await _notificationService.Create(new CreateUpdateNotificationRequest
        {
            Content = content,
            Message = message,
            LinkToNotification = $"/chat/group/{groupId}",
            EntityId = entityId,
            NotificationType = NotificationType.Chat
        });
    }
}
