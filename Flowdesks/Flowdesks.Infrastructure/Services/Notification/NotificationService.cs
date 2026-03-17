using AutoMapper;
using Flowdesks.Application.Hubs.Notification;
using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Models.Notification;
using Flowdesks.Application.Requests;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Application.Responses.Notification;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Notification;
using Flowdesks.Shared.Wrapper;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Infrastructure.Services.Notification;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ICurrentUserService _currentUserService;

    public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> hubContext, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _hubContext = hubContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<string>> Create(CreateUpdateNotificationRequest notification)
    {
        try
        {
            var entity = _mapper.Map<Domain.Entities.Notification.Notification>(notification);
            var response = _unitOfWork.Repository<Domain.Entities.Notification.Notification>().Add(entity);

            await _unitOfWork.SaveAsync();

            NotifyUsers(true);

            return await Result<string>.SuccessAsync("Notification created successfully");
        }
        catch (Exception ex)
        {
            return await Result<string>.FailAsync(ex.Message);
        }
    }

    private void NotifyUsers(bool hasNotifications)
    {
        BackgroundJob.Enqueue(() => SendNotification(hasNotifications));
    }

    public async Task SendNotification(bool hasNotifications)
    {
        await _hubContext.Clients.All.SendAsync("NotificationCreated", hasNotifications);
    }

    public async Task<Result<string>> Delete(Guid notificationId)
    {
        try
        {
            var response = _unitOfWork.Repository<Domain.Entities.Notification.Notification>().GetById(notificationId);

            if (response != null)
            {
                _unitOfWork.Repository<Domain.Entities.Notification.Notification>().Delete(response.Id);

                await _unitOfWork.SaveAsync();

                NotifyUsers(false);

                return await Result<string>.SuccessAsync("Notification deleted successfully");
            }

            return await Result<string>.FailAsync($"Notification with Id : {notificationId} not found");
        }
        catch (Exception ex)
        {
            return await Result<string>.FailAsync(ex.Message);
        }
    }

    public async Task<Result<NotificationDto>> GetByEntityIdAsync(string entityId)
    {
        try
        {
            var response = await _unitOfWork.Repository<Domain.Entities.Notification.Notification>().FirstOrDefaultAsync(x => x.EntityId.Equals(entityId));

            if (response != null)
            {
                var notification = _mapper.Map<NotificationDto>(response);

                return await Result<NotificationDto>.SuccessAsync(notification);
            }

            return await Result<NotificationDto>.FailAsync($"Notification with entityId : {entityId} not found");
        }
        catch (Exception ex)
        {
            return await Result<NotificationDto>.FailAsync(ex.Message);
        }
    }

    public async Task<Result<NotificationDto>> GetByIdAsync(Guid notificationId)
    {
        try
        {
            var response = _unitOfWork.Repository<Domain.Entities.Notification.Notification>().GetById(notificationId);

            if (response != null)
            {
                var notification = _mapper.Map<NotificationDto>(response);

                return await Result<NotificationDto>.SuccessAsync(notification);
            }

            return await Result<NotificationDto>.FailAsync($"Notification with Id : {notificationId} not found");
        }
        catch (Exception ex)
        {
            return await Result<NotificationDto>.FailAsync(ex.Message);
        }
    }

    public async Task<Result<NotificationPagedResponse>> GetListAsync(PagedRequest request)
    {
        try
        {
            var userId = _currentUserService.UserId;

            var query = _unitOfWork.Repository<Domain.Entities.Notification.Notification>().Entities()
                .Where(x => x.EntityId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.StringSearch))
            {
                string searchQuery = request.StringSearch.ToLower();

                query = query.Where(user =>
                    user.Content.ToLower().Contains(searchQuery) ||
                    user.Message.ToLower().Contains(searchQuery)
                );
            }

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.ApplySorting(request.SortColumn, request.SortOrder);
            }

            var totalCount = await query.LongCountAsync();
            var unreadCount = await query.Where(x => !x.IsRead).LongCountAsync();

            PaginatedResult<Domain.Entities.Notification.Notification> pagedNotifications = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);

            var mappedNotifications = _mapper.Map<PaginatedResult<NotificationDto>>(pagedNotifications);

            var response = new NotificationPagedResponse
            {
                TotalCount = totalCount,
                List = mappedNotifications,
                UnreadCount = unreadCount
            };

            return await Result<NotificationPagedResponse>.SuccessAsync(response);
        }
        catch (Exception ex)
        {
            return await Result<NotificationPagedResponse>.FailAsync(ex.Message);
        }

    }

    public async Task<Result<string>> Delete(Guid entityId, NotificationType type)
    {
        try
        {
            var response = await _unitOfWork.Repository<Domain.Entities.Notification.Notification>().FirstOrDefaultAsync(x => x.EntityId.Equals(entityId) && x.NotificationType.Equals(type.ToString()));

            if (response != null)
            {
                _unitOfWork.Repository<Domain.Entities.Notification.Notification>().Delete(response.Id);

                await _unitOfWork.SaveAsync();

                NotifyUsers(true);

                return await Result<string>.SuccessAsync("Success");
            }

            return await Result<string>.FailAsync($"Notification not found");
        }
        catch (Exception ex)
        {
            return await Result<string>.FailAsync(ex.Message);
        }
    }

    public async Task<Result<string>> MarkAsReadAsync(Guid? notificationId)
    {
        try
        {
            if (notificationId.HasValue)
            {
                var notification = await _unitOfWork.Repository<Domain.Entities.Notification.Notification>().GetByIdAsync(notificationId.Value);
                if (notification == null)
                    return await Result<string>.FailAsync($"Notification with Id : {notificationId} not found");

                notification.ReadOn = DateTime.UtcNow;
                notification.IsRead = true;
                _unitOfWork.Repository<Domain.Entities.Notification.Notification>().Update(notification);
            }
            else
            {
                var notifications = _unitOfWork.Repository<Domain.Entities.Notification.Notification>().Entities().Where(x => !x.IsRead);
                foreach (var notification in notifications)
                {
                    notification.ReadOn = DateTime.UtcNow;
                    notification.IsRead = true;
                    _unitOfWork.Repository<Domain.Entities.Notification.Notification>().Update(notification);
                }
            }

            await _unitOfWork.SaveAsync();
            NotifyUsers(true);

            return await Result<string>.SuccessAsync(notificationId.HasValue ? "Notification marked as read" : "All notifications marked as read");
        }
        catch (Exception ex)
        {
            return await Result<string>.FailAsync(ex.Message);
        }
    }
}