using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.CourseLocalRoles
{
    public class RequestUpdateModel : CourseLocalRolesBasePageModel
    {
        public RequestUpdateModel(CourseLocalRoleInteractor courseLocalRoleInteractor) : base(courseLocalRoleInteractor) { }

        public Response? QueryResult { get; set; }
        public LocalRoleDto? FoundLocalRole { get; set; }
        public int CourseId { get; set; }

        public async Task<IActionResult> OnGet(int courseId, int localRoleId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (courseId == 0 || localRoleId == 0) return;

                var response = await CourseLocalRoleInteractor.GetByCourseAndLocalIdAsync(courseId, localRoleId);

                if(response.Success && response.Value != null)
                {
                    FoundLocalRole = response.Value;
                    CourseId = courseId;
                }

                QueryResult = response;
            });
        }

        public async Task OnPost(
            int requesterUserId,
            int courseId,
            int localRoleId,
            string localRoleSign,
            string localRoleName,
            string viewAccess,
            string manageAccess,
            string editAccess,
            string removeAccess,
            string inviteAccess,
            string kickAccess,
            string editRolesAccess
            )
        {
            bool viewAccessValue = string.IsNullOrWhiteSpace(viewAccess) ? false : true;
            bool manageAccessValue = string.IsNullOrWhiteSpace(manageAccess) ? false : true;
            bool editAccessValue = string.IsNullOrWhiteSpace(editAccess) ? false : true;
            bool removeAccessValue = string.IsNullOrWhiteSpace(removeAccess) ? false : true;
            bool inviteAccessValue = string.IsNullOrWhiteSpace(inviteAccess) ? false : true;
            bool kickAccessValue = string.IsNullOrWhiteSpace(kickAccess) ? false : true;
            bool editRolesAccessValue = string.IsNullOrWhiteSpace(editRolesAccess) ? false : true;

            var localRoleDto = new LocalRoleDto()
            {
                Id = localRoleId,
                Sign = localRoleSign,
                Name = localRoleName,
                ViewAccess = viewAccessValue,
                ManageInternalAccess = manageAccessValue,
                EditAccess = editAccessValue,
                RemoveAccess = removeAccessValue,
                InviteAccess = inviteAccessValue,
                KickAccess = kickAccessValue,
                EditRolesAccess = editRolesAccessValue
            };

            QueryResult = await CourseLocalRoleInteractor.RequestUpdateLocalRoleAsync(requesterUserId, courseId, localRoleDto);
        }
    }
}
