using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Chat.Group;
using Flowdesks.Application.Responses.Chat.Groups;
using Flowdesks.Application.Specifications.Chat;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Chats.Groups.Query;

public class GetAllGroupQuery : IRequestHandler<GroupPagingRequest, Result<PaginatedResult<GroupResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllGroupQuery(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<GroupResponse>>> Handle(GroupPagingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            GroupFilterSpecification spec = new(request);

            var query = _unitOfWork.Repository<Group>().Entities()
                .Include(x => x.GroupUsers)
                .Include(x => x.GroupMessages)
                .Specify(spec);

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.ApplySorting(request.SortColumn, request.SortOrder);
            }

            var groups = await query
                .Select(groupResponse => new GroupResponse
                {
                    Id = groupResponse.Id,
                    GroupName = groupResponse.GroupName,
                    CreatedBy = groupResponse.CreatedBy,
                    Members = groupResponse.GroupUsers.Count,
                    Unread = GetUnreadCount(groupResponse, request.UserId)
                })
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return Result<PaginatedResult<GroupResponse>>.Success(groups);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<GroupResponse>>.Fail(ex.Message);
        }
    }

    private static int GetUnreadCount(Group group, Guid userId)
    {
        var userLastRead = group.GroupUsers.FirstOrDefault(x => x.UserId.Equals(userId)).LastReadOn;

        if (userLastRead != null)
        {
            return group.GroupMessages.Count(x => x.CreatedOn > userLastRead);
        }

        return group.GroupMessages.Count(x => !x.CreatedBy.Equals(userId.ToString()));
    }
}
