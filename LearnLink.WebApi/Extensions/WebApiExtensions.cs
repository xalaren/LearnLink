using LearnLink.Application.Helpers;
using LearnLink.WebApi.Helpers;

namespace LearnLink.WebApi.Extensions
{
    public static class WebApiExtensions
    {
        public static async void UseSeedData(this WebApplication app)
        {
            var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            var scope = scopedFactory.CreateAsyncScope();

            var service = scope.ServiceProvider.GetRequiredService<SeedData>();
            var registerConfig = scope.ServiceProvider.GetRequiredService<RegisterConfig>();
            
            await service.InitializeAdminRole();
            await service.InitializeUserRole();
            await service.InitializeModeratorLocalRole();
            await service.InitializeMemberLocalRole();

            var adminAuthData = registerConfig.GetAdminDefaultAuthenticationData(app.Configuration);


            await service.InitializeAdmin(adminAuthData.Nickname, adminAuthData.Password);

            await scope.DisposeAsync();
        }

        public static void UseInternalStorage(this WebApplication app) {
            var directoryStore = new DirectoryStore(app.Environment.ContentRootPath);
            Directory.CreateDirectory(directoryStore.InternalStorageDirectory);
        }
    }
}
