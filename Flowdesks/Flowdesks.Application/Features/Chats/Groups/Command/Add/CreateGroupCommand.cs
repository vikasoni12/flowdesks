using AutoMapper;
using Flowdesks.Application.Interfaces.Identity;
using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Chat.Group;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Shared.Constants.Chats;
using Flowdesks.Shared.Notification;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Chats.Groups.Command;

public class CreateGroupCommand : AddUpdateGroupRequest, IRequest<Result<int>>
{
    public List<Guid> UserIds { get; set; }
}

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;


    public CreateGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _notificationService=notificationService;
    }

    public async Task<Result<int>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var group = _mapper.Map<Group>(request);

            request.UserIds ??= new List<Guid>();

            request.UserIds.Add(new Guid(_currentUserService.UserId));

            group.GroupUsers = request.UserIds.Distinct().Select(userId => new GroupUser { UserId = userId }).ToList();

            _unitOfWork.Repository<Group>().Add(group);

            List<NotificationSetting> notificationSettings = await _unitOfWork.Repository<NotificationSetting>().Entities()
                                                .Where(x => request.UserIds.Contains((Guid) x.UserId) &&
                                                 (x.MessageType == ChatMessageConstant.Channels ||
                                                 x.MessageType == ChatMessageConstant.Both)).ToListAsync();

            if(notificationSettings.Any())
            {
                foreach (var userNotificationSetting in notificationSettings)
                {
                    if (userNotificationSetting != null)
                    {
                        await CreateNotification($"{group.GroupName}", $"You have been successfully added to the group '{group.GroupName}'.",
                            group.Id.ToString(), userNotificationSetting.UserId.ToString());
                    }
                }
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<int>.Success();
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
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
