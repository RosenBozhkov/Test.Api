using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.EnvHelper;

#nullable disable warnings

namespace Common.Helpers.Internal;

/// <summary>
/// Helper for getting the address for deployment
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public static class DeployHelper
{
    private const string DefaultAddress = "http://0.0.0.0:80";

    /// <summary>
    /// Returns the deployment address of the service. If deploy_listen is specified as an environment
    /// variable, then it will be taken. If not, the Default address is used.
    /// Valid format are "http://localhost:5001;https://localhost:5002"
    /// </summary>
    /// <returns></returns>
    public static string GetDeployAddress()
    {
        return EnvironmentHelper.DeployListen ?? DefaultAddress;
    }
}