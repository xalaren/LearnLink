namespace LearnLink.Core.Entities
{
    public class LocalRole : Role
    {
        public bool ViewAccess { get; set; }
        public bool EditAcess { get; set; }
        public bool RemoveAcess { get; set; }
        public bool ManageInternalAccess { get; set; }
    }
}
