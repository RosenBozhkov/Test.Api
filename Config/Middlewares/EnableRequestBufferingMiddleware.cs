using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using inacs.v8.nuget.DevAttributes;

namespace Config.Middlewares;

/// <summary>
/// Middleware for enabling request buffering
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public class EnableRequestBufferingMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="next"></param>
    public EnableRequestBufferingMiddleware(RequestDelegate next) => _next = next;

    /// <summary>
    /// Invocation
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        await _next(context).ConfigureAwait(false);
    }
}