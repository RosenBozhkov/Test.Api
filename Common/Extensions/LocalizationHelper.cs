using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Builder;

namespace Common.Extensions;

/// <summary>
/// Helper for localization
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public static class LocalizationHelper
{
    /// <summary>
    /// Registers the localization of the service
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = new[] { "en-US" };
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);

        return app;
    }
}