using Flowdesks.Application.Requests.Documents;
using FluentValidation;

namespace Flowdesks.Application.Validators.Documents
{
    public class DocumentByEntityTypeValidator : AbstractValidator<DocumentPagingRequest>
    {
        public DocumentByEntityTypeValidator()
        {
            RuleFor(x => x.EntityId).NotNull().NotEmpty();
            RuleFor(x => x.EntityType).NotNull().NotEmpty();
        }
    }
}
