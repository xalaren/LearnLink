using LearnLink.Core.Entities.ContentEntities;

namespace LearnLink.Core.Entities
{
    public class Section
    {
        private const int ORDER_DEFAULT_VALUE = 1;
        private int order = ORDER_DEFAULT_VALUE;
        public int Id { get; init; }
        public string? Title { get; set; }

        public int Order
        {
            get => order;
            set
            {
                if (value < ORDER_DEFAULT_VALUE)
                {
                    order = ORDER_DEFAULT_VALUE;
                    return;
                }

                order = value;
            }
        }
        
        public int? TextContentId { get; init; }
        public TextContent? TextContent { get; init; }
        
        public int? FileContentId { get; init; }
        public FileContent? FileContent { get; init; }
        
        public int? CodeContentId { get; init; }
        public CodeContent? CodeContent { get; init; }
    }
}
