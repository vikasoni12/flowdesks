using Flowdesks.Application.Interfaces.SystemPreferences;
using Flowdesks.Application.Requests;
using Flowdesks.Application.Validators;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Flowdesks.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IRequiredFieldService _requiredFieldsService;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, IRequiredFieldService requiredFieldsService)
    {
        _validators = validators;
        _requiredFieldsService = requiredFieldsService;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            var genericType = GetCreateEditRequestSubclassGenericType(request);
            var validationContext = new ValidationContext<TRequest>(request);

            if (genericType != null)
            {
                Type validatorType = typeof(CreateRequestValidator<>).MakeGenericType(genericType);
                var validator = Activator.CreateInstance(validatorType, _requiredFieldsService);

                var validationResult = ((IValidator<TRequest>)validator).Validate(validationContext);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            var failures = _validators
                .Select(v => v.Validate(validationContext))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                }).Select(x => new ValidationFailure(x.Key, string.Join(",", x.Values)))
                .ToList();


            if (failures.Any())
                throw new ValidationException(failures);

            return next();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public Type GetCreateEditRequestSubclassGenericType<T>(T request)
    {
        Type requestType = request.GetType();

        while (requestType != null && (!requestType.IsGenericType || requestType.GetGenericTypeDefinition() != typeof(CreateEditRequest<>)))
        {
            requestType = requestType.BaseType;
        }

        if (requestType == null)
        {
            return null;
        }

        Type genericTypeArgument = requestType.GetGenericArguments()[0];

        return genericTypeArgument;
    }
}
