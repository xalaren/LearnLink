using PandaRedu.Domain.Constants;
using PandaRedu.Domain.Entities.Abstractions;
using PandaRedu.Domain.Entities.Users.Enumerations;
using PandaRedu.Domain.Entities.Users.Identifiers;
using PandaRedu.Domain.Guards;
using PandaRedu.Domain.Guards.Clauses;

namespace PandaRedu.Domain.Entities.Users.Models;

/// <summary>
/// <see cref="Role"/> entity representation describing a user role and its flags.
/// </summary>
public class Role : Entity<RoleId>
{
    /// <summary>
    /// Maximum length allowed for the role name.
    /// </summary>
    public const int NameMaxLength = TextLengthConstants.Normal;

    /// <summary>
    /// Identifier of the <see cref="Role"/> entity.
    /// </summary>
    public override RoleId Id { get; protected init; }

    /// <summary>
    /// Role display name.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// True when this role is a system-defined role and cannot be modified freely.
    /// </summary>
    public bool IsSystem { get; private set; }

    /// <summary>
    /// True when this role grants administrator privileges.
    /// </summary>
    public bool IsAdmin { get; private set; }

    private readonly List<User> _users = [];

    /// <summary>
    /// Navigation collection of <see cref="Models.User"/>s assigned to this role.
    /// </summary>
    public IReadOnlyCollection<User> Users => _users.AsReadOnly();

    protected Role() { }

    private Role(RoleId id, string name, bool isSystem, bool isAdmin)
    {
        Id = id;
        Name = name;
        IsSystem = isSystem;
        IsAdmin = isAdmin;
    }

    /// <summary>
    /// Creates a new non-system <see cref="Role"/>.
    /// </summary>
    /// <param name="name">Role name</param>
    /// <param name="isAdmin">Whether the role has administrator privileges</param>
    /// <returns>New <see cref="Role"/> entity</returns>
    public static Role Create(string name, bool isAdmin)
    {
        Guard.For(name).AgainstEmpty().AgainstOverflow(NameMaxLength);
        return new Role
        (
            id: RoleId.New(),
            name: name,
            isSystem: false,
            isAdmin: isAdmin
        );
    }

    /// <summary>
    /// Creates the predefined system administrator <see cref="Role"/>.
    /// </summary>
    public static Role CreateSystemAdmin()
    {
        return new Role
        (
            id: PredefinedRoles.Administrator.Value,
            name: PredefinedRoles.Administrator.Name,
            isSystem: true,
            isAdmin: true
        );
    }

    /// <summary>
    /// Creates the predefined system user <see cref="Role"/>.
    /// </summary>
    public static Role CreateSystemUser()
    {
        return new Role
        (
            id: PredefinedRoles.User.Value,
            name: PredefinedRoles.User.Name,
            isSystem: true,
            isAdmin: false
        );
    }

    /// <summary>
    /// Renames the role.
    /// </summary>
    /// <param name="name">New role name</param>
    public void Rename(string name)
    {
        Guard.For(name).AgainstEmpty().AgainstOverflow(NameMaxLength);
        Name = name;
    }
}
