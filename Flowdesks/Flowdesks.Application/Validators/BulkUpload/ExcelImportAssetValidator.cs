using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.SystemPreferences;
using Flowdesks.Application.Requests.Asset;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Requests.BulkUpload;
using Flowdesks.Application.Requests.Stocks;
using Flowdesks.Application.Requests.Supplier;
using Flowdesks.Application.Requests.Technicians;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Validators.BulkUpload;
public class ExcelImportAssetValidator : AbstractValidator<BulkAssetRequest>
{
    private readonly IRequiredFieldService _requiredFieldsService;
    private readonly IDuplicateRecordsForBulkService _duplicateservice;
    public ExcelImportAssetValidator(IRequiredFieldService requiredFieldsService, IDuplicateRecordsForBulkService duplicateservice)
    {
        _requiredFieldsService = requiredFieldsService;
        _duplicateservice = duplicateservice;
        var requiredFields = _requiredFieldsService.GetFieldsByEntityType("Asset");

        foreach (var field in requiredFields)
        {
            RuleFor(x => GetPropertyValue(x, field)).NotEmpty().WithMessage($"The {field} field is required.").OverridePropertyName(field);
        }

        RuleFor(x => x.LifeSpan).Must(BeValidDecimalOrNull).WithMessage("Life Span must be a valid number or null."); 
        RuleFor(x => x.PurchaseCost).Must(BeValidDecimalOrNull).WithMessage("Purchase Cost must be a valid number or null.");
    }
    private object GetPropertyValue(BulkAssetRequest model, string propertyName)
    {
        var propertyInfo = model.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        return propertyInfo?.GetValue(model, null);
    }
    private bool BeValidDecimalOrNull(decimal? value)
    {
        return !value.HasValue || (value.HasValue && value.Value >= 0);
    }
}
