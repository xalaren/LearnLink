using CoursesPrototype.Shared.Exceptions;

namespace CoursesPrototype.Shared.ToClientData.DataTransferObjects;

public class UserDto
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
                throw new ForClientSideBaseException("Никнейм не был заполнен");
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
                throw new ForClientSideBaseException("Фамилия не была заполнена");
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
                throw new ForClientSideBaseException("Имя не было заполнено");
            }

            name = value;
        }
    }
}
