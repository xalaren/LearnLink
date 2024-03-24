using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Roles
{
    public class FindByNameModel : RolesBasePageModel
    {
        public FindByNameModel(RoleInteractor roleInteractor) : base(roleInteractor) { }
        
        public Response<RoleDto?>? QueryResult { get; set; }
        public RoleDto? FoundRole { get; set; }
        
        public async Task<IActionResult> OnGet(string? roleName)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (roleName == null) return;

                QueryResult = await RoleInteractor.GetRoleByNameAsync(roleName);

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    FoundRole = QueryResult.Value;
                }
            });
        }
    }
}
