using inacs.v8.nuget.DevAttributes;
using System;

namespace Common.Helpers;

/// <summary>
/// Exception helper
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public static class ExceptionHelper
{
    /// <summary>
    /// Gets most inner exception
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public static string GetMostInnerException(Exception exception)
    {
        if (exception.InnerException == null)
        {
            return exception.Message;
        }

        return GetMostInnerException(exception.InnerException);
    }
}