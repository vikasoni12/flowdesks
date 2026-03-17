using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Requests.BulkUpload;
using Flowdesks.Application.Requests.Site;
using FluentValidation;

namespace Flowdesks.Application.Validators.BulkUpload;

public class ExcelImportLocationValidator : AbstractValidator<BulkLocationRequest>
{

    private readonly IDuplicateRecordsForBulkService _duplicateservice;
    public ExcelImportLocationValidator(IDuplicateRecordsForBulkService duplicateservice)
    {
        _duplicateservice = duplicateservice;

        RuleFor(x => x.Name)
        .NotNull().WithMessage("Name can't be null")
        .NotEmpty().WithMessage("Name can't be empty");

        RuleFor(x => x.Floor)
        .NotNull().WithMessage("Floor can't be null")
        .NotEmpty().WithMessage("Floor can't be empty");

        RuleFor(x => x.Description)
        .NotNull().WithMessage("Description can't be null")
        .NotEmpty().WithMessage("Description can't be empty")
        .MaximumLength(250);

        RuleFor(x => x.BuildingId)
        .NotNull().WithMessage("Building Name and Code can't be null")
        .NotEmpty().WithMessage("Building Name and Code can't be empty");

    }


}