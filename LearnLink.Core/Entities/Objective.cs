using LearnLink.Core.Entities.ContentEntities;

namespace LearnLink.Core.Entities
{
    public class Objective
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        
        public int? FileContentId { get; set; }
        public FileContent? FileContent { get; set; }
    }
}
