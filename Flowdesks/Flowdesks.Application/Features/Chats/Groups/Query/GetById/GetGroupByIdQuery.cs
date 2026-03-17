using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Responses.Chat.Groups;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Shared.Constants.Common;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Chats.Groups.Query;

public class GetGroupByIdQuery : IRequest<Result<GroupDetailResponse>>
{
    public Guid Id { get; set; }
}

public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, Result<GroupDetailResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGroupByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GroupDetailResponse>> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var group = await _unitOfWork.Repository<Group>().Entities()
               .Include(x => x.GroupUsers).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

            if (group == null)
            {
                return Result<GroupDetailResponse>.Fail(ApplicationConstants.NotFound);
            }

            var response = _mapper.Map<GroupDetailResponse>(group);

            return Result<GroupDetailResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<GroupDetailResponse>.Fail(ex.Message);
        }
    }
}
