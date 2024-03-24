using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Roles
{
    public class FindByIdModel : RolesBasePageModel
    {
        public FindByIdModel(RoleInteractor roleInteractor) : base(roleInteractor) { }
        
        public Response<RoleDto?>? QueryResult { get; set; }
        public RoleDto? FoundRole { get; set; }
        
        public async Task<IActionResult> OnGet(int roleId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (roleId == 0) return;

                QueryResult = await RoleInteractor.GetRoleByIdAsync(roleId);

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    FoundRole = QueryResult.Value;
                }
            });
        }
    }
}
