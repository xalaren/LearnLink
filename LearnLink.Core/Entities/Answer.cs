using LearnLink.Core.Entities.ContentEntities;

namespace LearnLink.Core.Entities
{
    public class Answer
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ObjectiveId { get; set; }
        public Objective Objective { get; set; } = null!;

        public int? TextContentId { get; set; }
        public TextContent? TextContent { get; set; }

        public int? FileContentId { get; set; }
        public FileContent? FileContent { get; set; }
    }
}
