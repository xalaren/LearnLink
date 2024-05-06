namespace LearnLink.Shared.DataTransferObjects
{
    public record ParticipantDto
        (
            int Id,
            string Nickname,
            string Name,
            string Lastname,
            string? AvatarUrl,
            LocalRoleDto LocalRole
        );
}
