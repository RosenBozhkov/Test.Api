using Business.Interfaces.v1;
using inacs.v8.nuget.DevAttributes;
using Common.Resources;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Common.Exceptions;

namespace Business.Implementations.v1;

/// <summary>
/// Custom Validator for Data Annotations or Fluent Api.
/// !!! Note: It is advised to use Fluent API
/// !!! Data Annotations will not work when the attributes are on the Constructor parameters, if the attribute
/// is not specified like this [property: Required]
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class ValidatorService : IValidatorService
{
    // As Fluent Api has a validator for each class, there are two strategies.
    // The first is one validator, which resolves the IValidator<T> at runtime, implemented here.
    // This gives us one service to be injected, however it is more error prone.
    // The second option is for us to directly inject the IValidator<T> from the constructor.
    // The negative is that you may need to inject multiple validators for each class.

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ValidatorService> _logger;

    /// <summary>
    /// Constructor with DI for validation service
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="logger"></param>
    public ValidatorService(IServiceProvider serviceProvider, ILogger<ValidatorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Validates the object by the chosen strategy - data annotations or fluent validation.
    /// If object is not valid will throw an exception.
    /// </summary>
    /// <param name="model"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="InvalidModelStateException"></exception>
    public void Validate<T>(T model)
    {
        object? validatorObject = _serviceProvider.GetService(typeof(IValidator<T>));

        if(validatorObject is null)
        {
            _logger.LogWarning("No registered validator for the type {Type}", typeof(IValidator<T>).FullName);
            throw new ServiceNotConfiguredException(Messages.GeneralErrorMessage);
        }

        IValidator<T> validator = (IValidator<T>)validatorObject;
        ValidationResult result = validator.Validate(model);

        if (!result.IsValid)
        {
            string[] errorMessages = result.Errors
                .Select(x => x.ErrorMessage)
                .ToArray();
            string message = string.Join(Constants.ErrorDelimiter, errorMessages);
            throw new InvalidModelStateException(message, errorMessages);
        }
    }

    /// <summary>
    /// Validates the object by the chosen strategy - data annotations or fluent validation.
    /// If object is not valid will return false, or else true.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="errorMessages"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool TryValidate<T>(T model, out string[] errorMessages)
    {
        object? validatorObject = _serviceProvider.GetService(typeof(IValidator<T>));

        errorMessages = Array.Empty<string>();
        if (validatorObject is null)
        {
            _logger.LogWarning("No registered validator for the type {Type}", typeof(IValidator<T>).FullName);
            return true;
        }

        IValidator<T> validator = (IValidator<T>)validatorObject;
        ValidationResult result = validator.Validate(model);

        if (!result.IsValid)
        {
            errorMessages = result.Errors
                .Select(x => x.ErrorMessage)
                .ToArray();
            return false;
        }

        return true;
    }
}