using Flowdesks.Application.Features.Chats.Groups.Command;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Domain.Entities.Chat;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Validators.Chat
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGroupValidator(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;

            RuleFor(x => x.UserIds)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("UserIds must not be null")
            .NotEmpty().WithMessage("Must add at least one member")
            .Must(HaveAtLeastOneValidMember).WithMessage("Must add at least two valid members");

            RuleFor(x => x.GroupName)
            .Must(HaveUniqueGroupName).WithMessage("Group with the same name already exists for the current user");
        }

        private bool HaveAtLeastOneValidMember(List<Guid> userIds)
        {
            var validUserIds = userIds?.Where(x => !x.ToString().Equals(_currentUserService.UserId)).Distinct().ToList();

            return validUserIds != null && validUserIds.Count > 1;
        }

        private bool HaveUniqueGroupName(string groupName)
        {
            // Check if there is no existing group with the same name created by the current user
            return !_unitOfWork.Repository<Group>().Entities().Include(x => x.GroupUsers)
                .Any(x => x.GroupName.Equals(groupName, StringComparison.OrdinalIgnoreCase) && x.GroupUsers.Any(u => u.UserId.ToString().Equals(_currentUserService.UserId)));
        }
    }
}
