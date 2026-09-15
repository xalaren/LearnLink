using RustyTail.Domain.Constants;
using RustyTail.Domain.Entities.Abstractions;
using RustyTail.Domain.Entities.Users.Enumerations;
using RustyTail.Domain.Entities.Users.Identifiers;
using RustyTail.Domain.Guards;
using RustyTail.Domain.Guards.Clauses;

namespace RustyTail.Domain.Entities.Users.Models;

/// <summary>
/// <see cref="User"/> entity representation containing profile information and relations
/// such as role, avatar and refresh tokens.
/// </summary>
public class User : Entity<UserId>
{
    /// <summary>
    /// Identifier of the <see cref="User"/> entity.
    /// </summary>
    public override UserId Id { get; protected init; }

    /// <summary>
    /// Maximum length for the <see cref="Nickname"/> property.
    /// </summary>
    public const int NicknameMaxLength = TextLengthConstants.Normal;

    /// <summary>
    /// Maximum length for the <see cref="Name"/> property.
    /// </summary>
    public const int NameMaxLength = TextLengthConstants.Normal;

    /// <summary>
    /// Maximum length for the <see cref="Lastname"/> property.
    /// </summary>
    public const int LastnameMaxLength = TextLengthConstants.Normal;

    /// <summary>
    /// User nickname (display name / handle).
    /// </summary>
    public string Nickname { get; private set; } = null!;

    /// <summary>
    /// User first name.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// User last name.
    /// </summary>
    public string Lastname { get; private set; } = null!;

    /// <summary>
    /// True when this user is a system user and not a regular application user.
    /// </summary>
    public bool IsSystem { get; private set; }

    /// <summary>
    /// Foreign key referencing the assigned <see cref="Models.Role"/>.
    /// </summary>
    public RoleId RoleId { get; private set; }

    /// <summary>
    /// Navigation property to the assigned <see cref="Models.Role"/>.
    /// </summary>
    public Role Role { get; private set; } = null!;

    /// <summary>
    /// Optional foreign key referencing the <see cref="Models.Avatar"/> file details.
    /// </summary>
    public AvatarId? AvatarId { get; private set; }

    /// <summary>
    /// Navigation property to the <see cref="Models.Avatar"/> entity.
    /// </summary>
    public Avatar? Avatar { get; private set; }

    private readonly List<RefreshToken> _refreshTokens = [];

    /// <summary>
    /// Navigation collection of <see cref="Models.RefreshToken"/>s issued for this user.
    /// </summary>
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

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

    /// <summary>
    /// Creates a new non-system <see cref="User"/> with the default user role.
    /// </summary>
    /// <param name="nickname">User nickname</param>
    /// <param name="name">User first name</param>
    /// <param name="lastname">User last name</param>
    /// <returns>New <see cref="User"/> entity</returns>
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

    /// <summary>
    /// Creates a predefined system administrator <see cref="User"/>.
    /// </summary>
    /// <param name="nickname">User nickname</param>
    /// <param name="name">User first name</param>
    /// <param name="lastname">User last name</param>
    /// <returns>New system <see cref="User"/> entity with administrator role</returns>
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

    /// <summary>
    /// Changes the user's nickname.
    /// </summary>
    /// <param name="nickname">New nickname</param>
    public void ChangeNickname(string nickname)
    {
        Guard.For(nickname).AgainstWhiteSpace().AgainstOverflow(NicknameMaxLength);
        Nickname = nickname;
    }

    /// <summary>
    /// Changes the user's first name.
    /// </summary>
    /// <param name="name">New first name</param>
    public void ChangeName(string name)
    {
        Guard.For(name).AgainstWhiteSpace().AgainstOverflow(NameMaxLength);
        Name = name;
    }

    /// <summary>
    /// Changes the user's last name.
    /// </summary>
    /// <param name="lastname">New last name</param>
    public void ChangeLastname(string lastname)
    {
        Guard.For(lastname).AgainstWhiteSpace().AgainstOverflow(LastnameMaxLength);
        Lastname = lastname;
    }

    /// <summary>
    /// Updates the user's profile fields (nickname, first name, last name).
    /// </summary>
    /// <param name="nickname">New nickname</param>
    /// <param name="name">New first name</param>
    /// <param name="lastname">New last name</param>
    public void ChangeProfile(string nickname, string name, string lastname)
    {
        Guard.For(nickname).AgainstWhiteSpace().AgainstOverflow(NicknameMaxLength);
        Guard.For(name).AgainstWhiteSpace().AgainstOverflow(NameMaxLength);
        Guard.For(lastname).AgainstWhiteSpace().AgainstOverflow(LastnameMaxLength);

        Nickname = nickname;
        Name = name;
        Lastname = lastname;
    }

    /// <summary>
    /// Changes the assigned role for the user.
    /// </summary>
    /// <param name="roleId">Identifier of the new <see cref="Models.Role"/> to assign</param>
    public void ChangeRole(RoleId roleId)
    {
        Guard.For(roleId).AgainstEmpty();
        RoleId = roleId;
    }

    /// <summary>
    /// Checks whether the user has the specified role.
    /// </summary>
    /// <param name="roleId">Role identifier to check</param>
    /// <returns>True when the user's role matches <paramref name="roleId"/></returns>
    public bool HasRole(RoleId roleId) => RoleId == roleId;
}