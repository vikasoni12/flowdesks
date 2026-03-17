using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.NotificationSettings;
using Flowdesks.Application.Responses.Buldings;
using Flowdesks.Application.Responses.NotificationSetting;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.NotificationSettings.Query.GetAll
{
    public class GetAllNotificationSettingQuery : IRequestHandler<NotificationSettingPagingRequest, Result<PaginatedResult<NotificationSettingResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllNotificationSettingQuery(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<NotificationSettingResponse>>> Handle(NotificationSettingPagingRequest request, CancellationToken cancellationToken)
        {
            try
            {             
                var query = _unitOfWork.Repository<NotificationSetting>().Entities().Where(x=>x.UserId== request.UserId);                                                  
                if (!string.IsNullOrEmpty(request.SortColumn))
                {
                    query = query.ApplySorting(request.SortColumn, request.SortOrder);
                }

                var response = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);

                var notificationSetting = _mapper.Map<PaginatedResult<NotificationSettingResponse>>(response);

                return Result<PaginatedResult<NotificationSettingResponse>>.Success(notificationSetting);
            }
            catch (Exception ex)
            {
                return Result<PaginatedResult<NotificationSettingResponse>>.Fail(ex.Message);
            }
        }
    }
}
