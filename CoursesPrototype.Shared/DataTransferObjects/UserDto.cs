namespace CoursesPrototype.Shared.DataTransferObjects;

public record UserDto
    (
        int Id,
        string Nickname,
        string Lastname,
        string Name
    );

//public class UserDto
//{
//    public int Id { get; set; }

//    public string Nickname { get; set; } = null!;

//    public string Lastname { get; set; } = null!;

//    public string Name { get; set; }
//}
