using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Roles
{
    public class ListModel : RolesBasePageModel
    {
        public ListModel(RoleInteractor roleInteractor) : base(roleInteractor) { }

        public Response<RoleDto[]>? QueryResult { get; set; }
        public RoleDto[]? Roles { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return await AuthRequiredAsync(async () =>
            {
                QueryResult = await RoleInteractor.GetAllRolesAsync();

                if (QueryResult.Success)
                {
                    Roles = QueryResult.Value;
                }
            });
        }
    }
}
