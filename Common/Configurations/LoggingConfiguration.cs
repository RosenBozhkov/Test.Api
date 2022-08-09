using System.Collections.Generic;

namespace Common.Configurations;

/// <summary>
/// Configuration containing logging information
/// </summary>
public class LoggingConfiguration
{
    /// <summary>
    /// Properties of each logger
    /// </summary>
    // ReSharper disable once CollectionNeverUpdated.Global
    public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// List of loggers to be overriden
    /// </summary>
    public string[] Override { get; set; } = System.Array.Empty<string>();

    /// <summary>
    /// Url for the loki 
    /// </summary>
    public string LokiUrl { get; set; } = string.Empty;
    /// <summary>
    /// Template for the loki logs
    /// </summary>
    public string LokiTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Labels included in loki
    /// </summary>
    public string[] LokiLabels { get; set; } = System.Array.Empty<string>();
        
    /// <summary>
    /// Output template for console
    /// </summary>
    public string ConsoleTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Output template for file
    /// </summary>
    public string FileTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Name template for file
    /// </summary>
    public string FileName { get; set; } = string.Empty;
        
    /// <summary>
    /// Flush interval to disk in seconds
    /// </summary>
    public int FlushInterval { get; set; }
        
    /// <summary>
    /// True if file logging is buffered
    /// </summary>
    public bool IsBuffered { get; set; }
        
    /// <summary>
    /// If true, will block when the queue is full, instead of dropping events.
    /// </summary>
    public bool WilLBlockWhenFull { get; set; }
        
    /// <summary>
    /// Rolling interval for file
    /// </summary>
    public string RollingInterval { get; set; } = string.Empty;

    /// <summary>
    /// Retained file count before deletion
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int RetainedFileCount { get; set; }

    /// <summary>
    /// Output template for stored procedures
    /// </summary>
    public string StoredProcedureTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Name template for stored procedure
    /// </summary>
    public string StoredProcedureName { get; set; } = string.Empty;
        
    /// <summary>
    /// Output template for Ip logs
    /// </summary>
    public string IpTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Name template for Ip logs
    /// </summary>
    public string IpName { get; set; } = string.Empty;
}