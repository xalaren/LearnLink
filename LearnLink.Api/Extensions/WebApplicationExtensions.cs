using LearnLink.Api.Middleware;

namespace LearnLink.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void UseUrlPrinter(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var _ = app.Services.GetRequiredService<UrlPrinter>().Start(app.Lifetime.ApplicationStopped);
        });
    }

    public static void UseExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}