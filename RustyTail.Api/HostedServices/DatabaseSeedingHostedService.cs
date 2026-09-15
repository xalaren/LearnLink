using RustyTail.Api.Configurations;
using RustyTail.Application.Users.Services;

namespace RustyTail.Api.HostedServices;

/// <summary>
/// Database seeeding hosted service
/// </summary>
/// <param name="scopedFactory">Service scope factory</param>
/// <param name="registerConfig">Default system register config</param>
/// <param name="configuration">Current configuration</param>
public class DatabaseSeedingHostedService(
    IServiceScopeFactory scopedFactory,
    DefaultSystemUserConfig registerConfig,
    IConfiguration configuration) : IHostedService
{
    /// <summary>
    /// Starts seeding task
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopedFactory.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<SeedingService>();

        var defaultSystemUser = registerConfig.GetDefaultSystemUser(configuration);

        await service.InitializeUserRole(cancellationToken);
        await service.InitializeAdministratorRole(cancellationToken);
        await service.InitializeSystemUser(defaultSystemUser, cancellationToken);
    }

    /// <summary>
    /// Stops seeding task
    /// </summary>
    /// <param name="cancellationToken"></param>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}