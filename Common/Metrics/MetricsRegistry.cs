using App.Metrics;
using App.Metrics.Counter;
using inacs.v8.nuget.DevAttributes;

namespace Common.Metrics;

/// <summary>
/// Helper class for all metric types
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public static class MetricsRegistry
{
    /// <summary>
    /// Counter for example
    /// </summary>
    public static readonly CounterOptions DemoCounter = new()
    {
        Name = "demo counter",
        MeasurementUnit = Unit.Calls,
        Tags = new MetricTags("type", "total"),
        ResetOnReporting = true
    };
}