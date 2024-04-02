using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.LocalRoles
{
    public class FindByIdModel : LocalRolesBasePageModel
    {
        public FindByIdModel(LocalRoleInteractor localRoleInteractor) : base(localRoleInteractor) { }

        public Response<LocalRoleDto?>? QueryResult { get; set; }

        public LocalRoleDto? FoundLocalRole { get; set; }

        public async Task<IActionResult> OnGet(int localRoleId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (localRoleId == 0) return;

                QueryResult = await LocalRoleInteractor.GetLocalRoleByIdAsync(localRoleId);

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    FoundLocalRole = QueryResult.Value;
                }
            });
        }
    }
}
