using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.LocalRoles
{
    public class ListModel : LocalRolesBasePageModel
    {
        public ListModel(LocalRoleInteractor localRoleInteractor) : base(localRoleInteractor) { }

        public Response<LocalRoleDto[]>? QueryResult { get; set; }

        public LocalRoleDto[]? LocalRoles { get; set; }

        public Task<IActionResult> OnGet()
        {
            return AuthRequiredAsync(async () =>
            {
                QueryResult = await LocalRoleInteractor.GetAllLocalRolesAsync();

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    LocalRoles = QueryResult.Value;
                }
            });
        }
    }
}
