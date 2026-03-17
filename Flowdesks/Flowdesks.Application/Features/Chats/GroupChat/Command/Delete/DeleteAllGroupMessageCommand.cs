using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Specifications.Chat;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Chats.GroupChat.Command;

public class DeleteAllGroupMessageCommand : IRequest<Result<int>>
{
    public Guid UserId { get; set; }
    public Guid GroupId { get; set; }
}

public class DeleteAllGroupMessageCommandHandler : IRequestHandler<DeleteAllGroupMessageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAllGroupMessageCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(DeleteAllGroupMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var groupUser = await _unitOfWork.Repository<GroupUser>().Entities()
            .FirstOrDefaultAsync(x => x.GroupId.Equals(request.GroupId) && x.UserId.Equals(request.UserId), cancellationToken: cancellationToken);

            if (groupUser != null)
            {
                groupUser.LastDeletedOn = DateTime.UtcNow;

                _unitOfWork.Repository<GroupUser>().Update(groupUser);

                await _unitOfWork.SaveAsync(cancellationToken);

                return await Result<int>.SuccessAsync("Success");
            }

            return await Result<int>.FailAsync("Group not found");

        }
        catch (Exception ex)
        {
            return await Result<int>.FailAsync(ex.Message);
        }
    }
}
