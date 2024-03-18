namespace LearnLink.Core.Entities
{
    public class LocalRole : Role
    {
        public bool ViewAccess { get; set; }
        public bool EditAcess { get; set; }
        public bool RemoveAcess { get; set; }
        public bool ManageInternalAccess { get; set; }
        public bool InviteAccess { get; set; }
        public bool KickAccess { get; set; }

        public override bool IsAdmin
        {
            get
            {
                return ViewAccess && EditAcess && RemoveAcess && ManageInternalAccess && InviteAccess && KickAccess;
            }
        }

        public int GetRolePriority()
        {
            int priority = 0;

            if (ViewAccess) priority++;

            if (ManageInternalAccess) priority++;

            if (InviteAccess) priority++;

            if(KickAccess) priority++;

            if (EditAcess && RemoveAcess) priority++;

            return priority;
        }

        public bool GetOnlyViewAccess()
        {
            return ViewAccess && !(EditAcess || RemoveAcess || ManageInternalAccess || InviteAccess);
        }
    }
}
