using LearnLink.Api.Configurations;
using LearnLink.Application.Services;

namespace LearnLink.Api.HostedServices;

public class DatabaseSeedingHostedService(
    IServiceScopeFactory scopedFactory,
    DefaultSystemUserConfig registerConfig,
    IConfiguration configuration) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopedFactory.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<SeedingService>();

        var defaultSystemUser = registerConfig.GetDefaultSystemUser(configuration);

        await service.InitializeUserRole();
        await service.InitializeAdministratorRole();
        await service.InitializeSystemUser(defaultSystemUser);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}