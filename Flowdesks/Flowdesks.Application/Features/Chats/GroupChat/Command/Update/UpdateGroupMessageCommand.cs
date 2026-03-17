using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Chat.GroupMessage;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Flowdesks.Application.Features.Chats.GroupChat.Command;

public class UpdateGroupMessageCommand : AddUpdateGroupMessageRequest, IRequest<Result<int>>
{
    [Required] public Guid Id { get; set; }
}

public class UpdateGroupMessageCommandHandler : IRequestHandler<UpdateGroupMessageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateGroupMessageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<int>> Handle(UpdateGroupMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var message = await _unitOfWork.Repository<GroupMessage>().GetByIdAsync(request.Id);

            if (message != null)
            {
                if (message.SenderId.ToString() != _currentUserService.UserId)
                {
                    return await Result<int>.FailAsync("Authorization failed");
                }

                _mapper.Map<AddUpdateGroupMessageRequest, GroupMessage>(request, message);

                _unitOfWork.Repository<GroupMessage>().Update(message);

                await _unitOfWork.SaveAsync(cancellationToken);

                return Result<int>.Success("Message updated successfully");
            }
            else
            {
                return Result<int>.Fail($"Message with {request.Id} not found");
            }

        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}
