using PandaRedu.Domain.Constants;
using PandaRedu.Domain.Entities.Abstractions;
using PandaRedu.Domain.Entities.Users.Identifiers;
using PandaRedu.Domain.Guards;
using PandaRedu.Domain.Guards.Clauses;

namespace PandaRedu.Domain.Entities.Users.Models;

/// <summary>
/// <see cref="Avatar"/> entity representation
/// </summary>
public class Avatar : Entity<AvatarId>, IFileDetails
{
    /// <summary>
    /// Extension max length
    /// </summary>
    public const int ExtensionMaxLength = TextLengthConstants.Short;
    public override AvatarId Id { get; protected init; }
    public UserId UserId { get; private set; }
    public User User { get; private set; } = null!;
    public string Name => $"{UserId}.{Extension}";
    public string Extension { get; private set; } = null!;
    public long Size { get; private set; }
    public string? ContentType { get; private set; }

    private Avatar(AvatarId id, UserId userId, string extension, long size, string? contentType = null)
    {
        Id = id;
        UserId = userId;
        Extension = extension;
        Size = size;
        ContentType = contentType;
    }

    protected Avatar() { }

    /// <summary>
    /// Creates new <see cref="Avatar"/> entity
    /// </summary>
    /// <param name="extension"><see cref="Avatar"/> extension</param>
    /// <param name="size"><see cref="Avatar"/> file size in bytes</param>
    /// <param name="userId"><see cref="Models.User"/> foreign key</param>
    /// <returns>New <see cref="Avatar"/> entity</returns>
    public static Avatar Create(string extension, long size, UserId userId)
    {
        Guard.For(userId).AgainstEmpty();
        Guard.For(extension).AgainstEmpty();

        return new Avatar
        (
            id: AvatarId.New(),
            userId: userId,
            extension: extension,
            size: size,
            contentType: $"image/{extension}"
        );
    }
}