using inacs.v8.nuget.DevAttributes;
using System;
using System.Diagnostics;

namespace Common.Helpers;

/// <summary>
/// Utility methods measuring action execution time
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public static class MeasureUtils
{
    /// <summary>
    /// Measure action
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static long Measure(Action action)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        action();

        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }
}