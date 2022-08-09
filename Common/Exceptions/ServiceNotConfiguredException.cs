using System.Net;
using inacs.v8.nuget.Core.Exceptions;
using inacs.v8.nuget.DevAttributes;

namespace Common.Exceptions;

/// <summary>
/// Exception thrown when a setting for the service is not correct
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class ServiceNotConfiguredException : ServerException
{
    /// <summary>
    /// Constructor with message and errors
    /// </summary>
    /// <param name="message"></param>
    public ServiceNotConfiguredException(string message) : base(message)
    {
    }

    /// <summary>
    /// Constructor with message, status code and errors
    /// </summary>
    /// <param name="message"></param>
    /// <param name="status"></param>
    public ServiceNotConfiguredException(string message, HttpStatusCode status) : base(message, status)
    {
    }
}