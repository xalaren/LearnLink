using LearnLink.Domain.Constants;
using LearnLink.Domain.Entities.Abstractions;
using LearnLink.Domain.Entities.Users.Enumerations;
using LearnLink.Domain.Entities.Users.Identifiers;
using LearnLink.Domain.Guards;
using LearnLink.Domain.Guards.Clauses;

namespace LearnLink.Domain.Entities.Users.Models;

public class Role : Entity<RoleId>
{
    public const int NameMaxLength = TextLengthConstants.Normal;

    public override RoleId Id { get; protected init; }
    public string Name { get; private set; } = null!;
    public bool IsSystem { get; private set; }
    public bool IsAdmin { get; private set; }

    private readonly List<User> _users = [];
    public IReadOnlyCollection<User> Users => _users.AsReadOnly();

    protected Role() { }

    private Role(RoleId id, string name, bool isSystem, bool isAdmin)
    {
        Id = id;
        Name = name;
        IsSystem = isSystem;
        IsAdmin = isAdmin;
    }

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

    public void Rename(string name)
    {
        Guard.For(name).AgainstEmpty().AgainstOverflow(NameMaxLength);
        Name = name;
    }
}
