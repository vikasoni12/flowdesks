using AutoMapper;
using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Notes;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Domain.Entities.Note;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Notification;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WorkOrderEntity = Flowdesks.Domain.Entities.WorkOrder.WorkOrder;

namespace Flowdesks.Application.Features.Notes.Command.Add;

public class AddNoteCommand : IRequestHandler<AddNoteRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;

    public AddNoteCommand(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _notificationService=notificationService;
    }

    public async Task<Result<int>> Handle(AddNoteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            request.UserId = !string.IsNullOrEmpty(_currentUserService.UserId) ? new Guid(_currentUserService.UserId) : (Guid?)null;

            Note note = _mapper.Map<Note>(request);

            _unitOfWork.Repository<Note>().Add(note);

            if(note.EntityType == EntityType.WorkOrder.ToString())
            {
                await NotifyWorkOrderOnNoteAdded(request);
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<int>.Success();
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }

    private async Task NotifyWorkOrderOnNoteAdded(AddNoteRequest noteRequest)
    {
        var workOrder = await _unitOfWork.Repository<WorkOrderEntity>()
            .FirstOrDefaultAsync(x => x.Id == noteRequest.EntityId);
        if (workOrder != null)
        {
            var userIds = new List<Guid?>()
                                    {
                                        workOrder.AssignedSupplierId,
                                        workOrder.AssignedTechnicianId,
                                        workOrder.AssignedUserId,
                                        Guid.TryParse(workOrder.CreatedBy, out Guid createdBy) ? (Guid?)createdBy : null
                                    }.Where(id => id.HasValue).Select(id => id.Value).Distinct().ToList();

            List<NotificationSetting> notificationSettings = await _unitOfWork.Repository<NotificationSetting>().Entities()
                .Where(x => userIds.Contains((Guid)x.UserId))
                .ToListAsync();

            if(notificationSettings.Any())
            {
                foreach (var setting in notificationSettings)
                {
                    if (setting.IsAllAssignNewNote ==true || setting.IsAllCreatedByMeNewNote == true)
                    {
                        await SendNotification(workOrder, setting.UserId.ToString(), "Note added",
                            $"A new note has been added to the work order {workOrder.WorkOrderId} for the problem '{workOrder.Problem}'.");
                    }
                }
            }
           
        }

    }

    private async Task SendNotification(WorkOrderEntity workOrder, string userId, string content, string message)
    {
        await _notificationService.Create(new CreateUpdateNotificationRequest
        {
            Content = content,
            Message = message,
            LinkToNotification = $"/work-orders/{workOrder.WorkOrderId}",
            EntityId = userId,
            NotificationType = NotificationType.Building
        });
    }
}