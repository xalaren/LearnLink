using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.CourseLocalRoles
{
    public class CreateModel : CourseLocalRolesBasePageModel
    {
        public CreateModel(CourseLocalRoleInteractor courseLocalRoleInteractor) : base(courseLocalRoleInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(
            int requesterUserId,
            int courseId,
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
            bool viewAccessValue = !string.IsNullOrWhiteSpace(viewAccess);
            bool manageAccessValue = !string.IsNullOrWhiteSpace(manageAccess);
            bool editAccessValue = !string.IsNullOrWhiteSpace(editAccess);
            bool removeAccessValue = !string.IsNullOrWhiteSpace(removeAccess);
            bool inviteAccessValue = !string.IsNullOrWhiteSpace(inviteAccess);
            bool kickAccessValue = !string.IsNullOrWhiteSpace(kickAccess);
            bool editRolesAccessValue = !string.IsNullOrWhiteSpace(editRolesAccess);


            var localRoleDto = new LocalRoleDto()
            {
                Id = 0,
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

            QueryResult = await CourseLocalRoleInteractor.RequestCreateAsync(requesterUserId, courseId, localRoleDto);
        }
    }
}
