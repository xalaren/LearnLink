namespace CoursesPrototype.Shared.ToClientData.DataTransferObjects;

public class UserDto
{
    private string nickname = null!;
    private string lastname = null!;
    private string name = null!;

    public int Id { get; set; }

    public string Nickname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Name { get; set; } = null!;
}
