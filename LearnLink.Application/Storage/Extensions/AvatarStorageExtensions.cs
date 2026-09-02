
namespace LearnLink.Application.Storage.Extensions;

public static class AvatarsStorageExtensions
{
    public static string ToUrl(this Avatar avatar) =>
        $"/{Storage.Api}/{Storage.Directory}/{Storage.Users}/{avatar.UserId}/{Storage.Images}/{avatar.Name}";
}