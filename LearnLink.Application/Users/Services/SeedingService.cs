using LearnLink.Application.Abstractions.Data;
using LearnLink.Application.Security.Providers;
using LearnLink.Application.Users.Models;
using LearnLink.Domain.Entities.Users.Enumerations;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Users.Services;

public class SeedingService(IApplicationDataContext context, IEncryptionProvider encryptionProvider)
{
    private readonly IApplicationDataContext _context = context;
    private readonly IEncryptionProvider encryptionProvider = encryptionProvider;

    public async Task InitializeSystemUser(InitializeSystemUserRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var userId = PredefinedUsers.SystemUser.Value;

        var exists = await _context
                .Users
                .AsNoTracking()
                .AnyAsync(user => user.Id == userId);

        if (exists) return;

        var user = User.CreateSystemAdmin(request.Nickname, request.Name, request.Lastname);

        var password = encryptionProvider.Encrypt(request.Password);
        var credentials = Credentials.Create(password, user.Id, true);

        _context.Users.Add(user);
        _context.Credentials.Add(credentials);

        await _context.CommitAsync(cancellationToken);
    }

    public async Task InitializeAdministratorRole(CancellationToken cancellationToken = default)
    {
        var predefinedAdmin = PredefinedRoles.Administrator;
        var roleId = predefinedAdmin.Value;

        var exists = await _context
            .Roles
            .AsNoTracking()
            .AnyAsync(role => role.Id == roleId, cancellationToken);

        if (exists) return;

        var adminRole = Role.CreateSystemAdmin();

        _context.Roles.Add(adminRole);

        await _context.CommitAsync(cancellationToken);
    }

    public async Task InitializeUserRole(CancellationToken cancellationToken = default)
    {
        var predefinedUser = PredefinedRoles.User;
        var roleId = predefinedUser.Value;

        var exists = await _context
             .Roles
             .AsNoTracking()
             .AnyAsync(role => role.Id == roleId, cancellationToken);

        if (exists) return;

        var userRole = Role.CreateSystemUser();

        _context.Roles.Add(userRole);

        await _context.CommitAsync(cancellationToken);
    }
}