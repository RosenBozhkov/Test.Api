namespace Common.Configurations;

/// <summary>
/// Configuration class for versions
/// </summary>
public class ApplicationConfiguration
{
    private const string Separator = ".";

    /// <summary>
    /// Assumes the default version of the api for the request, when no version header is specified
    /// </summary>
    public bool AssumeDefaultVersion { get; set; }
    /// <summary>
    /// Header key for version
    /// </summary>
    public string VersionHeaderKey { get; set; } = string.Empty;
    /// <summary>
    /// The default version of the API
    /// </summary>
    public string DefaultApiVersion { get; set; } = string.Empty;
    /// <summary>
    /// Major version of the API
    /// </summary>
    public int DefaultMajorApiVersion => int.Parse(DefaultApiVersion.Split(Separator)[0]);
    /// <summary>
    /// Minor version of the API
    /// </summary>
    public int DefaultMinorApiVersion => int.Parse(DefaultApiVersion.Split(Separator)[1]);
    /// <summary>
    /// Content length restriction for requests in bytes
    /// </summary>
    public int ContentLengthRestriction { get; set; }
    /// <summary>
    /// Health endpoint url
    /// </summary>
    public string HealthEndpoint { get; set; } = string.Empty;
}