using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Site;
using FluentValidation;

namespace Flowdesks.Application.Validators.Site
{
    public class CreateSiteValidator : AbstractValidator<CreateSiteRequest>
    {
        public CreateSiteValidator(IUnitOfWork unitOfWork)
        {

            RuleFor(x => x.Name)
            .NotNull().WithMessage("Name can't be null")
            .NotEmpty().WithMessage("Name can't be empty");

            RuleFor(x => x.Code)
            .NotNull().WithMessage("Code can't be null")
            .NotEmpty().WithMessage("Code can't be empty");

            RuleFor(x => x.Address)
            .NotNull().WithMessage("Address can't be null")
            .NotEmpty().WithMessage("Address can't be empty")
            .MaximumLength(250);


        }


    }
}
