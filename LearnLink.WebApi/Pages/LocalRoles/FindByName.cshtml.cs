using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.LocalRoles
{
    public class FindByNameModel : LocalRolesBasePageModel
    {
        public FindByNameModel(LocalRoleInteractor localRoleInteractor) : base(localRoleInteractor) { }

        public Response<LocalRoleDto?>? QueryResult { get; set; }

        public LocalRoleDto? FoundLocalRole { get; set; }

        public async Task<IActionResult> OnGet(string? localRoleName)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(localRoleName)) return;

                QueryResult = await LocalRoleInteractor.GetLocalRoleByNameAsync(localRoleName);

                if (QueryResult.Success && QueryResult.Value != null)
                {
                    FoundLocalRole = QueryResult.Value;
                }
            });
        }
    }
}
