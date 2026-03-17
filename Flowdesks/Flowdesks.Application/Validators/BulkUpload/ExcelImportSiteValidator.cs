using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.BulkUpload;
using Flowdesks.Application.Requests.Site;
using FluentValidation;

namespace Flowdesks.Application.Validators.BulkUpload;

public class ExcelImportSiteValidator : AbstractValidator<BulkSiteRequest>
{

    private readonly IDuplicateRecordsForBulkService _duplicateservice;
    public ExcelImportSiteValidator(IDuplicateRecordsForBulkService duplicateservice)
    {
        _duplicateservice = duplicateservice;

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

        RuleFor(x => x.TelephoneNumber)
        .NotNull().WithMessage("Code can't be null")
        .NotEmpty().WithMessage("Code can't be empty").Matches(@"^\d+$").WithMessage("Telephone number must contain only digits"); ;

        RuleFor(x => x.Code).MustAsync(async (x, cancellation) => await _duplicateservice.CheckSiteCode(x)).WithMessage("duplicate code not allowed");
    }


}