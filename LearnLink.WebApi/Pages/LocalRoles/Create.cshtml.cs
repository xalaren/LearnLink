using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.LocalRoles
{
    public class CreateModel : LocalRolesBasePageModel
    {
        public CreateModel(LocalRoleInteractor localRoleInteractor) : base(localRoleInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(
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
                Id: 0,
                Sign: localRoleSign,
                Name: localRoleName,
                ViewAccess: viewAccessValue,
                ManageInternalAccess: manageAccessValue,
                EditAccess: editAccessValue,
                RemoveAccess: removeAccessValue,
                InviteAccess: inviteAccessValue,
                KickAccess: kickAccessValue
            );

            QueryResult = await LocalRoleInteractor.CreateLocalRoleAsync(localRoleDto);
        }
    }
}
