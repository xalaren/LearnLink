using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Roles
{
    public class UpdateModel : RolesBasePageModel
    {
        public UpdateModel(RoleInteractor roleInteractor) : base(roleInteractor) { }

        public Response? QueryResult { get; set; }

        public RoleDto? RoleDto { get; set; }

        public string? CheckProp { get; set; }

        public async Task<IActionResult> OnGet(int roleId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (roleId == 0) return;

                var result = await RoleInteractor.GetRoleByIdAsync(roleId);

                if(result.Success)
                {
                    RoleDto = result.Value;
                    return;
                }

                QueryResult = result;

               
            });
        }

        public async Task OnPost(int roleId, string name, string sign, string? isAdmin)
        {
            bool admin = string.IsNullOrWhiteSpace(isAdmin) ? false : true;
            var roleDto = new RoleDto(roleId, name, sign, admin);

            QueryResult = await RoleInteractor.UpdateRoleAsync(roleDto);
        }
    }
}
