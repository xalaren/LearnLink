using RustyTail.Api.Middleware;

namespace RustyTail.Api.Extensions;

/// <summary>
/// WebApplication extension helpers for registering hosted services and middleware.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Starts the <see cref="UrlPrinter"/> service when the application starts.
    /// </summary>
    public static void UseUrlPrinter(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var _ = app.Services.GetRequiredService<UrlPrinter>().Start(app.Lifetime.ApplicationStopped);
        });
    }

    /// <summary>
    /// Adds exception handling middleware to the pipeline.
    /// </summary>
    public static void UseExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
