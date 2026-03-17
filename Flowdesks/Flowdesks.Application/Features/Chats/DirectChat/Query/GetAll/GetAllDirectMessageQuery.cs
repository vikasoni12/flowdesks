using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Chat.DirectMessage;
using Flowdesks.Application.Responses.Chat.DirectMessage;
using Flowdesks.Application.Specifications.Chat;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Chats.DirectChat.Query;

public class GetAllDirectMessageQuery : IRequestHandler<DirectMessagePagingRequest, Result<PaginatedResult<DirectMessageResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetAllDirectMessageQuery(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PaginatedResult<DirectMessageResponse>>> Handle(DirectMessagePagingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            DirectMessageFilterSpecification spec = new(request);

            var query = _unitOfWork.Repository<Domain.Entities.Chat.DirectMessage>().Entities()
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Include(x => x.Attachment).Specify(spec);

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.ApplySorting(request.SortColumn, request.SortOrder);
            }

            var messages = await query.OrderBy(x => x.CreatedOn)
                .ProjectTo<DirectMessageResponse>(_mapper.ConfigurationProvider).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            var currentUserId = _currentUserService.UserId;

            var otherpersonId = request.SenderId.ToString().Equals(currentUserId) ? request.ReceiverId : request.SenderId;

            BackgroundJob.Enqueue(() => MarkMessagesAsRead(currentUserId, otherpersonId));

            return Result<PaginatedResult<DirectMessageResponse>>.Success(messages);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<DirectMessageResponse>>.Fail(ex.Message);
        }
    }

    public async Task MarkMessagesAsRead(string currentUserId, Guid otherpersonId)
    {
        var messagesToMarkAsRead = _unitOfWork.Repository<Domain.Entities.Chat.DirectMessage>().Entities().Where(x => x.SenderId.Equals(otherpersonId) && x.ReceiverId.ToString().Equals(currentUserId));

        if (messagesToMarkAsRead.Any())
        {
            foreach (var message in messagesToMarkAsRead)
            {
                message.IsRead = true;
            }

            _unitOfWork.Repository<Domain.Entities.Chat.DirectMessage>().UpdateRange(messagesToMarkAsRead);
            await _unitOfWork.SaveAsync();
        }
    }
}