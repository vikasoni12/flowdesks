using Flowdesks.Application.Interfaces.SystemPreferences;
using Flowdesks.Application.Requests;
using FluentValidation;
using System.Reflection;

namespace Flowdesks.Application.Validators;

public class CreateRequestValidator<T> : AbstractValidator<CreateEditRequest<T>>
{
    private readonly IRequiredFieldService _requiredFieldsService;

    public CreateRequestValidator(IRequiredFieldService requiredFieldsService)
    {
        _requiredFieldsService = requiredFieldsService;

        var genericType = typeof(T).Name;

        var requiredFields = _requiredFieldsService.GetFieldsByEntityType(genericType);

        foreach (var field in requiredFields)
        {
            RuleFor(x => GetPropertyValue(x, field)).NotEmpty().WithMessage($"The {field} field is required.").OverridePropertyName(field);
        }
    }

    private object GetPropertyValue(CreateEditRequest<T> model, string propertyName)
    {
        var propertyInfo = model.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        return propertyInfo?.GetValue(model, null);
    }
}
