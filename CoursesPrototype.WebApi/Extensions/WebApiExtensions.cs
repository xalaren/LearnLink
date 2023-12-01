using CoursePrototype.WebApi;
using CoursesPrototype.Application.Helpers;
using Microsoft.Extensions.Hosting;

namespace CoursesPrototype.WebApi.Extensions
{
    public static class WebApiExtensions
    {
        public static async void UseSeedData(this WebApplication app)
        {
            var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            var scope = scopedFactory.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<SeedData>();

            await service.InitializeAdminRole();
            await service.InitializeUserRole();

            await service.InitializeAdmin();
        }
    }
}
