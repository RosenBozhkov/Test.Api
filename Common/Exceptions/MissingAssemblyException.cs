using System.Net;
using inacs.v8.nuget.Core.Exceptions;
using inacs.v8.nuget.DevAttributes;

#pragma warning disable S3925 //No need for serialization

namespace Common.Exceptions;

/// <summary>
/// Exception thrown when a configuration assembly is missing
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class MissingAssemblyException : ServerException
{
    /// <summary>
    /// Constructor with message
    /// </summary>
    /// <param name="message">The message sent back to the client</param>
    public MissingAssemblyException(string message) : base(message)
    {
    }

    /// <summary>
    /// Constructor with message and status code
    /// </summary>
    /// <param name="message"></param>
    /// <param name="status">Status code to be set for the request</param>
    public MissingAssemblyException(string message, HttpStatusCode status) : base(message, status)
    {
    }
}