using LearnLink.Application.Transaction;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class PermissionService
    {
        private readonly IUnitOfWork unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> GetPermissionAsync(int userId, int courseId, 
            bool toView = false, 
            bool toEdit = false, 
            bool toRemove = false, 
            bool toManageInternal = false,
            bool toInvite = false,
            bool toKick = false)
        {
            var user = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            await unitOfWork.Users.Entry(user)
                .Reference(u => u.Role)
                .LoadAsync();

            if (user.Role.IsAdmin)
            {
                return true;
            }

            var userCourseRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId);

            if (userCourseRole == null)
            {
                return false;
            }

            await unitOfWork.UserCourseLocalRoles.Entry(userCourseRole)
                .Reference(u => u.LocalRole)
                .LoadAsync();

            return
                (userCourseRole.LocalRole.ViewAccess && toView) ||
                (userCourseRole.LocalRole.EditAcess && toEdit) ||
                (userCourseRole.LocalRole.RemoveAcess && toRemove) ||
                (userCourseRole.LocalRole.ManageInternalAccess && toManageInternal) ||
                (userCourseRole.LocalRole.InviteAccess && toInvite) ||
                (userCourseRole.LocalRole.KickAccess && toKick);
        }
    }
}
