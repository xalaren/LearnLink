namespace LearnLink.Shared.DataTransferObjects
{
    public class LocalRoleDto() 
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sign { get; set; } = string.Empty;
        public bool ViewAccess { get; set; }
        public bool EditAccess { get; set; }
        public bool RemoveAccess { get; set; }
        public bool ManageInternalAccess { get; set; }
        public bool InviteAccess { get; set; }
        public bool KickAccess { get; set; }
        public bool EditRolesAccess { get; set; }
        public bool IsAdmin { get; }
    }
}
