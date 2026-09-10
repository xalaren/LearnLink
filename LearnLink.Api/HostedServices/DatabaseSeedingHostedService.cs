using LearnLink.Api.Configurations;
using LearnLink.Application.Users.Services;

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

        await service.InitializeUserRole(cancellationToken);
        await service.InitializeAdministratorRole(cancellationToken);
        await service.InitializeSystemUser(defaultSystemUser, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}