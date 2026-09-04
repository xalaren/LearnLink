using LearnLink.Application.Data;
using LearnLink.Application.Security;
using LearnLink.Domain.Entities.Users.Enumerations;
using LearnLink.Domain.Entities.Users.Models;
using LearnLink.Shared.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Services;

public class SeedingService(IApplicationDataContext context, IEncryptionProvider encryptionProvider)
{
    private readonly IApplicationDataContext _context = context;
    private readonly IEncryptionProvider encryptionProvider = encryptionProvider;

    public async Task InitializeSystemUser(DefaultSystemUser request)
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

        await _context.CommitAsync();
    }

    public async Task InitializeAdministratorRole()
    {
        var predefinedAdmin = PredefinedRoles.Administrator;
        var roleId = predefinedAdmin.Value;

        var exists = await _context
            .Roles
            .AsNoTracking()
            .AnyAsync(role => role.Id == roleId);

        if (exists) return;

        var adminRole = Role.CreateSystemAdmin();

        _context.Roles.Add(adminRole);

        await _context.CommitAsync();
    }

    public async Task InitializeUserRole()
    {
        var predefinedUser = PredefinedRoles.User;
        var roleId = predefinedUser.Value;

        var exists = await _context
             .Roles
             .AsNoTracking()
             .AnyAsync(role => role.Id == roleId);

        if (exists) return;

        var userRole = Role.CreateSystemUser();

        _context.Roles.Add(userRole);

        await _context.CommitAsync();
    }
}