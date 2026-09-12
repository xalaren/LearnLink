using LearnLink.Domain.Abstractions;
using LearnLink.Domain.Entities.Users.Identifiers;

namespace LearnLink.Domain.Entities.Users.Enumerations;

/// <summary>
/// Enumeration of default predefined roles
/// </summary>
public class PredefinedRoles : Enumeration<PredefinedRoles, RoleId>
{
    private PredefinedRoles(RoleId value, string name) : base(value, name) { }

    /// <summary>
    /// Administrator role
    /// </summary>
    public static readonly PredefinedRoles Administrator = new(RoleId.Parse("e3165241-85dc-446d-a4f1-047779712886"), nameof(Administrator));

    /// <summary>
    /// User role
    /// </summary>
    public static readonly PredefinedRoles User = new(RoleId.Parse("e8369b7e-a4e1-40dc-9912-738cd2ea5a0e"), nameof(User));
}
