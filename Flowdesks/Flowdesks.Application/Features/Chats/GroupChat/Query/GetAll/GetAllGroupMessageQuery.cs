using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Chat.GroupMessage;
using Flowdesks.Application.Responses.Chat.GroupMessage;
using Flowdesks.Application.Specifications.Chat;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Chats.GroupChat.Query;

public class GetAllGroupMessageQuery : IRequestHandler<GroupMessagePagingRequest, Result<PaginatedResult<GroupMessageResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetAllGroupMessageQuery(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PaginatedResult<GroupMessageResponse>>> Handle(GroupMessagePagingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = _currentUserService.UserId;

            GroupMessageFilterSpecification spec = new(request, currentUserId);

            var groupUser = await _unitOfWork.Repository<GroupUser>().FirstOrDefaultAsync(x => x.UserId.ToString().Equals(currentUserId) && x.GroupId.Equals(request.GroupId));

            if (groupUser == null)
            {
                return Result<PaginatedResult<GroupMessageResponse>>.Fail("Unauthorized member");
            }

            var query = _unitOfWork.Repository<GroupMessage>().Entities()
                .Include(x => x.Group)
                    .ThenInclude(x => x.GroupUsers)
                .Include(x => x.Sender).Include(x => x.Attachment)
                .WhereIf(groupUser.LastDeletedOn != null, x => x.CreatedOn > groupUser.LastDeletedOn)
                .Specify(spec);

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.ApplySorting(request.SortColumn, request.SortOrder);
            }

            var messages = await query.OrderBy(x => x.CreatedOn).ProjectTo<GroupMessageResponse>(_mapper.ConfigurationProvider).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            BackgroundJob.Enqueue(() => UpdateLastRead(currentUserId, request.GroupId));

            return Result<PaginatedResult<GroupMessageResponse>>.Success(messages);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<GroupMessageResponse>>.Fail(ex.Message);
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
}
