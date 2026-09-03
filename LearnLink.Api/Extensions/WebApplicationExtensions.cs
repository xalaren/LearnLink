using LearnLink.Api.Configurations;
using LearnLink.Application.Services;
using LearnLink.Application.Storages;

namespace LearnLink.Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async void UseSeedData(this WebApplication app)
        {
            var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            var scope = scopedFactory.CreateAsyncScope();

            var service = scope.ServiceProvider.GetRequiredService<SeedingService>();
            var registerConfig = scope.ServiceProvider.GetRequiredService<DefaultSystemUserConfig>();

            var defaultSystemUser = registerConfig.GetDefaultSystemUser(app.Configuration);

            await service.InitializeUserRole();
            await service.InitializeAdministratorRole();
            await service.InitializeSystemUser(defaultSystemUser);

            await scope.DisposeAsync();
        }

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
}
