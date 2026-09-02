namespace LearnLink.Domain.Entities.Abstractions
{
    public interface IFileDetails
    {
        string Name { get; }
        string Extension { get; }
        long Size { get; }
        string? ContentType { get; }
    }
}
