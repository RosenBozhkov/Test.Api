using inacs.v8.nuget.DevAttributes;

namespace Common.Extensions;

/// <summary>
/// String extension class
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public static class StringExtensions
{
    private const string HexDigits = "0123456789abcdef";

    /// <summary>
    /// Truncates a string
    /// </summary>
    /// <param name="str"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string? Truncate(this string? str, int length)
    {
        if (length <= 0 || str is null)
        {
            return str;
        }

        if (str.Length > length)
        {
            return str.Substring(0, length);
        }

        return str;
    }
        
    /// <summary>
    /// Checks if the char is a valid hex lowercase digit
    /// </summary>
    /// <param name="ch"></param>
    /// <returns></returns>
    public static bool IsHexadecimal(this char ch)
    {
        return HexDigits.Contains(ch);
    }
}