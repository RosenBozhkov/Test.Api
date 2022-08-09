using inacs.v8.nuget.DevAttributes;
using Serilog.Core;
using Serilog.Events;

namespace Config.SerilogUtils;

/// <summary>
/// This is used to create a new property in Logs called 'DefaultLevel'
/// So that we can map Serilog levels to Log4Net levels - so log files stay consistent
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class DefaultLevelEnricher : ILogEventEnricher
{
    /// <summary>
    /// Enriches the logger with our own log levels
    /// </summary>
    /// <param name="logEvent"></param>
    /// <param name="propertyFactory"></param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var log4NetLevel = logEvent.Level switch
        {
            LogEventLevel.Debug => "DEBUG",
            LogEventLevel.Error => "ERROR",
            LogEventLevel.Fatal => "FATAL",
            LogEventLevel.Information => "INFO",
            LogEventLevel.Verbose => "ALL",
            LogEventLevel.Warning => "WARN",
            _ => string.Empty
        };
            
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("DefaultLevel", log4NetLevel));
    }
}