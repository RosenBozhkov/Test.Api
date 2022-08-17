using System.Net;
using inacs.v8.nuget.Core.Exceptions;
using inacs.v8.nuget.DevAttributes;

namespace Common.Exceptions;

/// <summary>
/// Exception thrown when a resource already exists
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]

public class InvalidNameException : ClientException
{
    /// <summary>
    /// Constructor with message and errors
    /// </summary>
    /// <param name="message"></param>
    public InvalidNameException(string message) : base(message)
    {
    }
}
