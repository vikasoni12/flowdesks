using Flowdesks.Application.Requests.Identity;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Flowdesks.Application.Validators.Identity
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator(IStringLocalizer<ForgotPasswordRequestValidator> localizer)
        {
            RuleFor(request => request.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["UserName is required"]);
        }
    }
}