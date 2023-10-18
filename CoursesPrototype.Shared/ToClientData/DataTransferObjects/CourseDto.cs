using CoursesPrototype.Shared.Exceptions;

namespace CoursesPrototype.Shared.ToClientData.DataTransferObjects
{
    public class CourseDto
    {
        private string title = string.Empty;

        public int Id { get; set; }
        public bool IsPublic { get; set; } = false;
        public string? Description { get; set; }

        public string Title
        {
            get => title;
            set
            {
                if(!string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ForClientSideBaseException("Заголовок не был заполнен");
                }

                title = value;
            }
        }


    }
}
