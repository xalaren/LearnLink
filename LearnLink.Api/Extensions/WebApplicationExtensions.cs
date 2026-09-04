using LearnLink.Application.Storages;

namespace LearnLink.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void UseInternalStorage(this WebApplication app) 
    {
        var internalDirectory = Storage.Instance(app.Environment.ContentRootPath).InternalDirectory;
        Directory.CreateDirectory(internalDirectory);
    }

    public static void UseUrlPrinter(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var _ = app.Services.GetRequiredService<UrlPrinter>().Start(app.Lifetime.ApplicationStopped);
        });
    }
}