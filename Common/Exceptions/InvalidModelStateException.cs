using System.Net;
using inacs.v8.nuget.Core.Exceptions;
using inacs.v8.nuget.DevAttributes;

namespace Common.Exceptions;

/// <summary>
/// Exception thrown when the model state is not valid
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class InvalidModelStateException : ClientException
{
    /// <summary>
    /// A list of all error messages
    /// </summary>
    public string[] Errors { get; }
        
    /// <summary>
    /// Constructor with message and errors
    /// </summary>
    /// <param name="message"></param>
    /// <param name="errors"></param>
    public InvalidModelStateException(string message, params string[] errors) : base(message)
    {
        Errors = errors;
    }

    /// <summary>
    /// Constructor with message, status code and errors
    /// </summary>
    /// <param name="message"></param>
    /// <param name="status"></param>
    /// <param name="errors"></param>
    public InvalidModelStateException(string message, HttpStatusCode status, params string[] errors) : base(message, status)
    {
        Errors = errors;
    }
}