using CoursesPrototype.Core.Exceptions;

namespace CoursesPrototype.Core.Entities
{
    public class Role
    {
        private string name = string.Empty;
        private string sign = string.Empty;

        public int Id { get; set; }
        public string Name
        {
            get => name;
            set
            {
                if (!string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidationException("Название роли не было заполнено");
                }

                name = value;
            }
        }

        public string Sign
        {
            get => sign;
            set
            {
                if (!string.IsNullOrWhiteSpace(sign) && string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidationException("Подпись роли не была заполнена");
                }

                sign = value.ToLower();
            }
        }
    }
}
