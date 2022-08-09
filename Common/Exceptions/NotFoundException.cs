using System.Net;
using inacs.v8.nuget.Core.Exceptions;
using inacs.v8.nuget.DevAttributes;

namespace Common.Exceptions;

/// <summary>
/// Exception thrown when a resource is not found
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class NotFoundException : ClientException
{
    /// <summary>
    /// Constructor with message and errors
    /// </summary>
    /// <param name="message"></param>
    public NotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Constructor with message, status code and errors
    /// </summary>
    /// <param name="message"></param>
    /// <param name="status"></param>
    public NotFoundException(string message, HttpStatusCode status) : base(message, status)
    {
    }
}