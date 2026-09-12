using LearnLink.Domain.Abstractions;
using LearnLink.Domain.Entities.Users.Identifiers;

namespace LearnLink.Domain.Entities.Users.Enumerations;

/// <summary>
/// Enumeration of default predefined users
/// </summary>
public class PredefinedUsers : Enumeration<PredefinedUsers, UserId>
{
    private PredefinedUsers(UserId value, string name) : base(value, name) { }

    /// <summary>
    /// System user
    /// </summary>
    public static readonly PredefinedUsers SystemUser = new(UserId.Parse("e132befd-463e-483c-92d3-06d0cf91790a"), nameof(SystemUser));
}
