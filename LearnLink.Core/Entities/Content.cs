using LearnLink.Core.Exceptions;

namespace LearnLink.Core.Entities
{
    public class Content
    {
        private int order = 1;

        public int Id { get; set; }
        public bool IsText { get; set; }
        public bool IsCodeBlock { get; set; }
        public bool IsFile { get; set; }

        public int Order
        {
            get => order;
            set
            {
                if(value < 1)
                {
                    throw new ValidationException("Порядок содержимого не может быть меньше 1");
                }

                order = value;
            }
        }

        public string Text { get; set; } = string.Empty;
        public string? FileName { get; set; }
    }
}
