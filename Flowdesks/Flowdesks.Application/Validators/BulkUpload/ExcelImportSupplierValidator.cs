using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.SystemPreferences;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Requests.BulkUpload;
using Flowdesks.Application.Requests.Supplier;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Validators.BulkUpload;
public class ExcelImportSupplierValidator : AbstractValidator<BulKSupplierRequest>
{
    private readonly IRequiredFieldService _requiredFieldsService;
    private readonly IDuplicateRecordsForBulkService _duplicateservice;
    public ExcelImportSupplierValidator(IRequiredFieldService requiredFieldsService, IDuplicateRecordsForBulkService duplicateservice)
    {
        _requiredFieldsService = requiredFieldsService;
        _duplicateservice = duplicateservice;
        var requiredFields = _requiredFieldsService.GetFieldsByEntityType("Supplier");

        foreach (var field in requiredFields)
        {
            RuleFor(x => GetPropertyValue(x, field)).NotEmpty().WithMessage($"The {field} field is required.").OverridePropertyName(field);
        }

        RuleFor(x => x.Code).MustAsync(async (x, cancellation) => await _duplicateservice.CheckSupplierCode(x)).WithMessage("duplicate Code not allowed");

    }
    private object GetPropertyValue(BulKSupplierRequest model, string propertyName)
    {
        var propertyInfo = model.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        return propertyInfo?.GetValue(model, null);
    }

}
