using System.Net;
using inacs.v8.nuget.Core.Exceptions;
using inacs.v8.nuget.DevAttributes;

namespace Common.Exceptions;

/// <summary>
/// Exception thrown when a telemetry trace id and span id are not valid
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class InvalidTraceException : ClientException
{
    /// <summary>
    /// Constructor with message
    /// </summary>
    /// <param name="message">The message sent back to the client</param>
    public InvalidTraceException(string message) : base(message)
    {
    }

    /// <summary>
    /// Constructor with message and status code
    /// </summary>
    /// <param name="message"></param>
    /// <param name="status">Status code to be set for the request</param>
    public InvalidTraceException(string message, HttpStatusCode status) : base(message, status)
    {
    }
}