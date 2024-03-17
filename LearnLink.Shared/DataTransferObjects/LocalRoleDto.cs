namespace LearnLink.Shared.DataTransferObjects
{
    public record LocalRoleDto(
        int Id,
        string Name,
        string Sign,
        bool ViewAccess,
        bool EditAccess,
        bool RemoveAccess,
        bool ManageInternalAccess,
        bool InviteAccess
    );
}
