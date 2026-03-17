using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Specifications.Chat;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Constants.Common;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Chats.Groups.Query;

public class LeaveGroupQuery : IRequest<Result<int>>
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }
}

public class LeaveGroupQueryHandler : IRequestHandler<LeaveGroupQuery, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public LeaveGroupQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(LeaveGroupQuery request, CancellationToken cancellationToken)
    {
        try
        {
            LeaveGroupFilterSpecification spec = new LeaveGroupFilterSpecification(request);

            var groupUser = _unitOfWork.Repository<GroupUser>().Entities().Specify(spec).FirstOrDefault();

            if (groupUser == null)
            {
                return Result<int>.Fail(ApplicationConstants.NotFound);
            }

            _unitOfWork.Repository<GroupUser>().Delete(groupUser.Id);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<int>.Success();
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}
