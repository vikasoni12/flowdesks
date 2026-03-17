using Flowdesks.Application.Requests.Chat.DirectMessage;
using FluentValidation;

namespace Flowdesks.Application.Validators.Chat
{
    public class SendMessageValidator : AbstractValidator<AddUpdateDirectMessageRequest>
    {
        public SendMessageValidator()
        {
            RuleFor(x => x.Content).NotNull().NotEmpty();
            RuleFor(x => x.ReceiverId).NotNull().NotEmpty();
        }
    }
}
