using System.Net;
using inacs.v8.nuget.Core.Exceptions;
using inacs.v8.nuget.DevAttributes;

namespace Common.Exceptions;

/// <summary>
/// Exception thrown when a additional price exceeds 10% of total price
/// </summary>
public class InvalidPriceException : ClientException
{
    /// <summary>
    /// Constructor with message and errors
    /// </summary>
    /// <param name="message"></param>
    public InvalidPriceException(string message) : base(message)
    {
    }
}
