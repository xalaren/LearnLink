using LearnLink.Core.Exceptions;

namespace LearnLink.Core.Entities
{
    public class LocalRole
    {
        private string name = string.Empty;
        private string sign = string.Empty;

        public int Id { get; set; }
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
                    throw new ValidationException("Название роли не было заполнено");
                }

                name = value;
            }
        }

        public string Sign
        {
            get => sign;
            set
            {
                if (!string.IsNullOrWhiteSpace(sign) && string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidationException("Подпись роли не была заполнена");
                }

                sign = value.ToLower();
            }
        }


        public bool ViewAccess { get; set; }
        public bool EditAcess { get; set; }
        public bool RemoveAccess { get; set; }
        public bool ManageInternalAccess { get; set; }
        public bool InviteAccess { get; set; }
        public bool KickAccess { get; set; }
        public bool EditRolesAccess { get; set; }
        
        public bool IsModerator => ViewAccess && EditAcess && RemoveAccess && ManageInternalAccess && InviteAccess && KickAccess && EditRolesAccess;
        public bool SystemRole { get; init; }

        public int GetRolePriority()
        {
            int priority = 0;

            if (ViewAccess) priority++;

            if (ManageInternalAccess) priority++;

            if (InviteAccess) priority++;

            if(KickAccess) priority++;

            if (EditAcess && RemoveAccess) priority++;

            if (EditRolesAccess) priority++;

            return priority;
        }
    }
}
