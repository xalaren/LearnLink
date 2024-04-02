using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.LocalRoles
{
    public class UpdateModel : LocalRolesBasePageModel
    {
        public UpdateModel(LocalRoleInteractor localRoleInteractor) : base(localRoleInteractor) { }

        public Response? QueryResult { get; set; }

        public LocalRoleDto? FoundLocalRole { get; set; }

        public async Task<IActionResult> OnGet(int localRoleId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (localRoleId == 0) return;

                var result =  await LocalRoleInteractor.GetLocalRoleByIdAsync(localRoleId);

                if(result.Success && result.Value != null)
                {
                    FoundLocalRole = result.Value;
                    return;
                }

                QueryResult = result;
            });
        }

        public async Task OnPost(
            int localRoleId,
            string localRoleSign,
            string localRoleName,
            string viewAccess,
            string manageAccess,
            string editAccess,
            string removeAccess,
            string inviteAccess,
            string kickAccess)
        {
            bool viewAccessValue = string.IsNullOrWhiteSpace(viewAccess) ? false : true;
            bool manageAccessValue = string.IsNullOrWhiteSpace(manageAccess) ? false : true;
            bool editAccessValue = string.IsNullOrWhiteSpace(editAccess) ? false : true;
            bool removeAccessValue = string.IsNullOrWhiteSpace(removeAccess) ? false : true;
            bool inviteAccessValue = string.IsNullOrWhiteSpace(inviteAccess) ? false : true;
            bool kickAccessValue = string.IsNullOrWhiteSpace(kickAccess) ? false : true;


            var localRoleDto = new LocalRoleDto(
                Id: localRoleId,
                Sign: localRoleSign,
                Name: localRoleName,
                ViewAccess: viewAccessValue,
                ManageInternalAccess: manageAccessValue,
                EditAccess: editAccessValue,
                RemoveAccess: removeAccessValue,
                InviteAccess: inviteAccessValue,
                KickAccess: kickAccessValue
            );

            QueryResult = await LocalRoleInteractor.UpdateLocalRoleAsync(localRoleDto);
        }
    }
}
