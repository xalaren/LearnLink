namespace LearnLink.Shared.DataTransferObjects
{
    public record RoleDto(
        int Id,
        string Name,
        string Sign,
        bool IsAdmin = false
    );
}
