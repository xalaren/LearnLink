using LearnLink.Core.Entities.Abstractions;
using LearnLink.Domain.Constants;
using LearnLink.Domain.Entities.Abstractions;
using LearnLink.Domain.Entities.Users.Identifiers;
using LearnLink.Domain.Entities.Users.Models;
using LearnLink.Domain.Exceptions;
using LearnLink.Domain.Guards;
using LearnLink.Domain.Guards.Clauses;

public class Avatar : Entity<AvatarId>, IFileDetails
{
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