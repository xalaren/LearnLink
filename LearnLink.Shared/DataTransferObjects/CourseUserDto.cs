namespace LearnLink.Shared.DataTransferObjects
{
    public record CourseUserDto
        (
            int Id,
            string Nickname,
            string Name,
            string Lastname,
            string? AvatarUrl,
            string LocalRoleName
        );
}
