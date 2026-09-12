using LearnLink.Domain.Entities.Abstractions;

namespace LearnLink.Domain.Entities.Users.Identifiers;

/// <summary>
/// Strongly typed primary key of <see cref="Credentials"/> entity
/// </summary>
/// <param name="Value">Guid representation</param>
public readonly record struct CredentialsId(Guid Value) : ITypedKey<CredentialsId, Guid>
{
    public static CredentialsId Empty() => new(Guid.Empty);
    public static CredentialsId New() => new(Guid.NewGuid());
    public static CredentialsId Parse(string value) => new(Guid.Parse(value));
    public static bool IsEmpty(CredentialsId credentialsId) => credentialsId.Value == Guid.Empty;
    public override string ToString() => Value.ToString();
}
