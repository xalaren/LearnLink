using LearnLink.Application.Helpers;
using LearnLink.Application.Transaction;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class PermissionService(IUnitOfWork unitOfWork)
    {
        public async Task<Permission> GetPermissionAsync(int userId, int courseId, 
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
                return new Permission(false);
            }

            await unitOfWork.Users.Entry(user)
                .Reference(u => u.Role)
                .LoadAsync();

            if (user.Role.IsAdmin)
            {
                return new Permission(true);
            }

            var userCourseRole = await unitOfWork.UserCourseLocalRoles
                .FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId);
            

            if (userCourseRole == null)
            {
                return new Permission(false);
            }

            await unitOfWork.UserCourseLocalRoles.Entry(userCourseRole)
                .Reference(role => role.LocalRole)
                .LoadAsync();
            
            bool access = (userCourseRole.LocalRole.ViewAccess && toView) ||
                          (userCourseRole.LocalRole.EditAcess && toEdit) ||
                          (userCourseRole.LocalRole.RemoveAccess && toRemove) ||
                          (userCourseRole.LocalRole.ManageInternalAccess && toManageInternal) ||
                          (userCourseRole.LocalRole.InviteAccess && toInvite) ||
                          (userCourseRole.LocalRole.KickAccess && toKick);

            return new Permission(access);
        }
    }
}
