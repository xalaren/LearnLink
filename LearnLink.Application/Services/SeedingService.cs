using LearnLink.Application.Repositories;
using LearnLink.Application.Security;
using LearnLink.Application.Transactions;
using LearnLink.Domain.Entities.Users.Enumerations;
using LearnLink.Domain.Entities.Users.Models;
using LearnLink.Shared.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Services;

public class SeedingService(IUnitOfWork unitOfWork, IEncryptionService encryptionService)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEncryptionService _encryptionService = encryptionService;

    public async Task InitializeSystemUser(DefaultSystemUser request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var userId = PredefinedUsers.SystemUser.Value;

        var exists = await _unitOfWork
                .Repository
                .Users
                .AsNoTracking()
                .AnyAsync(user => user.Id == userId);

        if (exists) return;

        var user = User.CreateSystemAdmin(request.Nickname, request.Name, request.Lastname);

        var password = _encryptionService.Encrypt(request.Password);
        var credentials = Credentials.Create(password, user.Id, true);

        _unitOfWork.Repository.Users.Add(user);
        _unitOfWork.Repository.Credentials.Add(credentials);

        await _unitOfWork.CommitAsync();
    }

    public async Task InitializeAdministratorRole()
    {
        var predefinedAdmin = PredefinedRoles.Administrator;
        var roleId = predefinedAdmin.Value;

        var exists = await _unitOfWork
            .Repository
            .Roles
            .AsNoTracking()
            .AnyAsync(role => role.Id == roleId);

        if (exists) return;

        var adminRole = Role.CreateSystemAdmin();

        _unitOfWork.Repository.Roles.Add(adminRole);

        await _unitOfWork.CommitAsync();
    }

    public async Task InitializeUserRole()
    {
        var predefinedUser = PredefinedRoles.User;
        var roleId = predefinedUser.Value;

        var exists = await _unitOfWork
             .Repository
             .Roles
             .AsNoTracking()
             .AnyAsync(role => role.Id == roleId);

        if (exists) return;

        var userRole = Role.CreateSystemUser();

        _unitOfWork.Repository.Roles.Add(userRole);

        await _unitOfWork.CommitAsync();
    }
}