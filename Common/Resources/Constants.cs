using inacs.v8.nuget.DevAttributes;

namespace Common.Resources;

/// <summary>
/// Project constants
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public static class Constants
{
    /// <summary>
    /// Error delimiter
    /// </summary>
    public const string ErrorDelimiter = "; ";
    /// <summary>
    /// Remote Ip address not found message
    /// </summary>
    public const string RemoteIpAddressNotFound = "Remote Ip address not found";
    /// <summary>
    /// No Framework specified message
    /// </summary>
    public const string NoFrameworkSpecified = "No framework specified";
}