using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.SystemPreferences;
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
public class ExcelImportTechnicianValidator : AbstractValidator<BulkTechnicianRequest>
{
    private readonly IRequiredFieldService _requiredFieldsService;
    private readonly IDuplicateRecordsForBulkService _duplicateservice;
    public ExcelImportTechnicianValidator(IRequiredFieldService requiredFieldsService, IDuplicateRecordsForBulkService duplicateservice)
    {
        _requiredFieldsService = requiredFieldsService;
        _duplicateservice = duplicateservice;
        var requiredFields = _requiredFieldsService.GetFieldsByEntityType("Technician");

        foreach (var field in requiredFields)
        {
            RuleFor(x => GetPropertyValue(x, field)).NotEmpty().WithMessage($"The {field} field is required.").OverridePropertyName(field);
        }

        RuleFor(x => x.IdNumber).MustAsync(async (x, cancellation) => await _duplicateservice.CheckTechnicianIdNumber(x)).WithMessage("duplicate IdNumber not allowed");

        RuleFor(x => x.MobileNumber)
           .Matches(@"^\d+$").WithMessage("Telephone number must contain only digits")
           .When(x => !string.IsNullOrEmpty(x.MobileNumber)).WithMessage("Telephone number must contain only digits");
    }
    private object GetPropertyValue(BulkTechnicianRequest model, string propertyName)
    {
        var propertyInfo = model.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        return propertyInfo?.GetValue(model, null);
    }

}
