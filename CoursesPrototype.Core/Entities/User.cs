using System.ComponentModel.DataAnnotations;

namespace CoursesPrototype.Core.Entities
{
    public class User
    {
        private string nickname = null!;
        private string lastname = null!;
        private string name = null!;

        public int Id { get; set; }

        public string Nickname
        {
            get => nickname;
            set
            {
                if (!string.IsNullOrWhiteSpace(nickname) && string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidationException("Никнейм не был заполнен");
                }

                nickname = value;
            }
        }

        public string Lastname
        {
            get => lastname;
            set
            {
                if (!string.IsNullOrWhiteSpace(lastname) && string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidationException("Фамилия не была заполнена");
                }

                lastname = value;
            }
        }

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
                    throw new ValidationException("Имя не было заполнено");
                }

                name = value;
            }
        }
    }
}