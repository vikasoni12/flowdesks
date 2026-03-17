using Flowdesks.Application.Requests.Documents;
using FluentValidation;

namespace Flowdesks.Application.Validators.Documents
{
    internal class AddNewDocumentValidator : AbstractValidator<AddDocumentRequest>
    {
        public AddNewDocumentValidator()
        {
            RuleFor(x => x.EntityId).NotNull().NotEmpty();
            RuleFor(x => x.EntityType).NotNull().NotEmpty();
        }
    }
}
