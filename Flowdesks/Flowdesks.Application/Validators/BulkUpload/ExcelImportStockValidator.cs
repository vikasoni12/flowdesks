using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.SystemPreferences;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Requests.BulkUpload;
using Flowdesks.Application.Requests.Stocks;
using Flowdesks.Application.Requests.Supplier;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Validators.BulkUpload;
public class ExcelImportStockValidator : AbstractValidator<BulkStockRequest>
{
    private readonly IRequiredFieldService _requiredFieldsService;
    private readonly IDuplicateRecordsForBulkService _duplicateservice;
    public ExcelImportStockValidator(IRequiredFieldService requiredFieldsService, IDuplicateRecordsForBulkService duplicateservice)
    {
        _requiredFieldsService = requiredFieldsService;
        _duplicateservice = duplicateservice;
        var requiredFields = _requiredFieldsService.GetFieldsByEntityType("Stock");

        foreach (var field in requiredFields)
        {
            RuleFor(x => GetPropertyValue(x, field)).NotEmpty().WithMessage($"The {field} field is required.").OverridePropertyName(field);
        }

        RuleFor(x => x.PartCode).MustAsync(async (x, cancellation) => await _duplicateservice.CheckStockPartCode(x)).WithMessage("duplicate PartCode not allowed");

        RuleFor(x => x.Quantity)
             .Must(BeValidDecimalOrNull).WithMessage("Quantity must be a valid number or null.");

        RuleFor(x => x.UnitCost)
            .Must(BeValidDecimalOrNull).WithMessage("UnitCost must be a valid number or null.");

        RuleFor(x => x.MinQuantity)
            .Must(BeValidDecimalOrNull).WithMessage("MinQuantity must be a valid number or null.");
    }
    private object GetPropertyValue(BulkStockRequest model, string propertyName)
    {
        var propertyInfo = model.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        return propertyInfo?.GetValue(model, null);
    }
    private bool BeValidDecimalOrNull(decimal? value)
    {
        return !value.HasValue || (value.HasValue && value.Value >= 0);
    }
}
