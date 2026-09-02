using LearnLink.Core.Entities.Abstractions;
using LearnLink.Domain.Constants;
using LearnLink.Domain.Entities.Users.Enumerations;
using LearnLink.Domain.Entities.Users.Identifiers;
using LearnLink.Domain.Guards;
using LearnLink.Domain.Guards.Clauses;

namespace LearnLink.Domain.Entities.Users.Models;

public class User : Entity<UserId>
{
    public override UserId Id { get; protected init; }
    public const int NicknameMaxLength = TextLengthConstants.Normal;
    public const int NameMaxLength = TextLengthConstants.Normal;
    public const int LastnameMaxLength = TextLengthConstants.Normal;

    public string Nickname { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Lastname { get; private set; } = null!;

    public bool IsSystem { get; private set; }
    
    public RoleId RoleId { get; private set; }
    public Role Role { get; private set; } = null!;

    public AvatarId? AvatarId { get; private set; }
    public Avatar? Avatar { get; private set; }

    private User(UserId id, string nickname, string name, string lastname, bool isSystem, RoleId roleId)
    {
        Id = id;
        Nickname = nickname;
        Name = name;
        Lastname = lastname;
        IsSystem = isSystem;
        RoleId = roleId;
    }

    protected User() {  }

    public static User Create(string nickname, string name, string lastname)
    {
        Guard.For(nickname).AgainstWhiteSpace().AgainstOverflow(NicknameMaxLength);
        Guard.For(name).AgainstWhiteSpace().AgainstOverflow(NameMaxLength);
        Guard.For(lastname).AgainstWhiteSpace().AgainstOverflow(LastnameMaxLength);

        return new User
        (
            id: UserId.New(),
            nickname: nickname,
            name: name,
            lastname: lastname,
            isSystem: false,
            roleId: PredefinedRoles.User.Value
        );
    }

    public static User CreateSystemAdmin(string nickname, string name, string lastname)
    {
        Guard.For(nickname).AgainstWhiteSpace().AgainstOverflow(NicknameMaxLength);
        Guard.For(name).AgainstWhiteSpace().AgainstOverflow(NameMaxLength);
        Guard.For(lastname).AgainstWhiteSpace().AgainstOverflow(LastnameMaxLength);

        return new User
        (
            id: PredefinedUsers.SystemUser.Value,
            nickname: nickname,
            name: name,
            lastname: lastname,
            isSystem: true,
            roleId: PredefinedRoles.Administrator.Value
        );
    }

    public void ChangeNickname(string nickname)
    {
        Guard.For(nickname).AgainstWhiteSpace().AgainstOverflow(NicknameMaxLength);
        Nickname = nickname;
    }

    public void ChangeName(string name)
    {
        Guard.For(name).AgainstWhiteSpace().AgainstOverflow(NameMaxLength);
        Name = name;
    }

    public void ChangeLastname(string lastname)
    {
        Guard.For(lastname).AgainstWhiteSpace().AgainstOverflow(LastnameMaxLength);
        Lastname = lastname;
    }

    public void ChangeProfile(string nickname, string name, string lastname)
    {
        Guard.For(nickname).AgainstWhiteSpace().AgainstOverflow(NicknameMaxLength);
        Guard.For(name).AgainstWhiteSpace().AgainstOverflow(NameMaxLength);
        Guard.For(lastname).AgainstWhiteSpace().AgainstOverflow(LastnameMaxLength);

        Nickname = nickname;
        Name = name;
        Lastname = lastname;
    }

    public void ChangeRole(RoleId roleId)
    {
        Guard.For(roleId).AgainstEmpty();
        RoleId = roleId;
    }

    public bool HasRole(RoleId roleId) => RoleId == roleId;
}