using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Chat.DirectMessage;
using Flowdesks.Application.Responses.Chat.DirectMessage;
using Flowdesks.Application.Specifications.Chat;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Buildings.Messagess.Query;

public class GetMessagesByUserIdQuery : UserMessageRequest
{
}

public class GetMessagesByUserIdQueryHandler : IRequestHandler<GetMessagesByUserIdQuery, Result<PaginatedResult<UserDirectMessagesResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMessagesByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<UserDirectMessagesResponse>>> Handle(GetMessagesByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            DirectMessageFilterSpecification spec = new(request);

            var query = _unitOfWork.Repository<DirectMessage>().Entities()
                 .Include(x => x.Sender)
                 .Include(x => x.Receiver)
                 .Specify(spec)
                 .GroupBy(x => new { OtherPersonId = (x.SenderId == request.UserId) ? x.ReceiverId : x.SenderId })
                 .Select(group => new
                 {
                     Message = group.OrderByDescending(x => x.CreatedOn).FirstOrDefault(),
                     UnreadCount = group.Count(x => x.ReceiverId == request.UserId && !x.IsRead)
                 });

            var messages = query.AsEnumerable().Select(dm => new UserDirectMessagesResponse
            {
                Id = (request.UserId == dm.Message.SenderId) ? dm.Message.ReceiverId : dm.Message.SenderId,
                Name = (request.UserId == dm.Message.SenderId) ? $"{dm.Message.Receiver.FirstName} {dm.Message.Receiver.LastName}" : $"{dm.Message.Sender.FirstName} {dm.Message.Sender.LastName}",
                Image = (request.UserId == dm.Message.SenderId) ? dm.Message.Receiver.ProfilePictureDataUrl : dm.Message.Sender.ProfilePictureDataUrl,
                Unread = dm.UnreadCount
            }).DistinctBy(user => user.Id).ToPaginatedEnumerableList(request.PageNumber, request.PageSize);

            return await Result<PaginatedResult<UserDirectMessagesResponse>>.SuccessAsync(messages);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<UserDirectMessagesResponse>>.Fail(ex.Message);
        }
    }
}
