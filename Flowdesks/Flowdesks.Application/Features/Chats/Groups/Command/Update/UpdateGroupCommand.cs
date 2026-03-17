using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Chat.Group;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Chats.Groups.Command;

public class UpdateGroupCommand : AddUpdateGroupRequest, IRequest<Result<int>>
{
    public Guid Id { get; set; }
}

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<int>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var group = await _unitOfWork.Repository<Group>().GetByIdAsync(request.Id);

            if (group != null)
            {
                if (!HaveUniqueGroupName(request.GroupName, request.Id))
                {
                    return Result<int>.Fail($"Duplicate group name");
                }

                _mapper.Map<AddUpdateGroupRequest, Group>(request, group);

                _unitOfWork.Repository<Group>().Update(group);

                await _unitOfWork.SaveAsync();

                return Result<int>.Success("Group updated successfully");
            }
            else
            {
                return Result<int>.Fail($"Group with {request.Id} not found");
            }

        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }

    private bool HaveUniqueGroupName(string groupName, Guid id)
    {
        // Check if there is no existing group with the same name created by the current user
        return !_unitOfWork.Repository<Group>().Entities().Any(x => !x.Id.Equals(id) && x.GroupName.Equals(groupName, StringComparison.OrdinalIgnoreCase) && x.CreatedBy.Equals(_currentUserService.UserId));
    }
}
