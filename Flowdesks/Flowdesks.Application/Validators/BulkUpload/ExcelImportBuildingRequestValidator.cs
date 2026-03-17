using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.SystemPreferences;
using Flowdesks.Application.Requests;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Requests.BulkUpload;
using FluentValidation;
using System.Reflection;

namespace Flowdesks.Application.Validators.BulkUpload;


public class ExcelImportBuildingRequestValidator : AbstractValidator<BulkBuildingRequest>
{
    private readonly IRequiredFieldService _requiredFieldsService;
    private readonly IDuplicateRecordsForBulkService _duplicateservice;
    public ExcelImportBuildingRequestValidator(IRequiredFieldService requiredFieldsService, IDuplicateRecordsForBulkService duplicateservice)
    {
        _requiredFieldsService = requiredFieldsService;
        _duplicateservice = duplicateservice;
        var requiredFields = _requiredFieldsService.GetFieldsByEntityType("Building");

        foreach (var field in requiredFields)
        {
            RuleFor(x => GetPropertyValue(x, field)).NotEmpty().WithMessage($"The {field} field is required.").OverridePropertyName(field);
        }

        RuleFor(x => x.Code).MustAsync(async (x, cancellation) => await _duplicateservice.CheckBuildingCode(x)).WithMessage("duplicate Code not allowed");

    }
    private object GetPropertyValue(BulkBuildingRequest model, string propertyName)
    {
        var propertyInfo = model.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        return propertyInfo?.GetValue(model, null);
    }

}
