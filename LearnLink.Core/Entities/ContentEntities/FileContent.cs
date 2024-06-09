namespace LearnLink.Core.Entities.ContentEntities
{
    public class FileContent
    {
        public int Id { get; init; }
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
    }
}