using Common.Exceptions;
using inacs.v8.nuget.DevAttributes;

namespace Business.Interfaces.v1;

/// <summary>
/// Model validation service
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public interface IValidatorService
{
    /// <summary>
    /// Validates the object by the chosen strategy - data annotations or fluent validation.
    /// If object is not valid will throw an exception.
    /// </summary>
    /// <param name="model"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="InvalidModelStateException"></exception>
    void Validate<T>(T model);
        
    /// <summary>
    /// Validates the object by the chosen strategy - data annotations or fluent validation.
    /// If object is not valid will return false, or else true.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="errorMessages"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    bool TryValidate<T>(T model, out string[] errorMessages);
}