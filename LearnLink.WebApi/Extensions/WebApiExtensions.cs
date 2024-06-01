using LearnLink.Application.Helpers;

namespace LearnLink.WebApi.Extensions
{
    public static class WebApiExtensions
    {
        public static async void UseSeedData(this WebApplication app)
        {
            var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            var scope = scopedFactory.CreateAsyncScope();

            var service = scope.ServiceProvider.GetRequiredService<SeedData>();
            
            await service.InitializeAdminRole();
            await service.InitializeUserRole();
            await service.InitializeModeratorLocalRole();
            await service.InitializeMemberLocalRole();

            var defaultAdminData = app.Configuration.AdminDefaultAuthenticationData();
            await service.InitializeAdmin(defaultAdminData.Nickname, defaultAdminData.Password);

            await scope.DisposeAsync();
        }

        public static void UseInternalStorage(this WebApplication app) {
            var directoryStore = new DirectoryStore(app.Environment.ContentRootPath);
            Directory.CreateDirectory(directoryStore.InternalStorageDirectory);
        }
    }
}
