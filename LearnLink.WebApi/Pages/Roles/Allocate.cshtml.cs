using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Roles
{
    public class AllocateModel : RolesBasePageModel
    {
        public AllocateModel(RoleInteractor roleInteractor) : base(roleInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int roleId, int userId)
        {
            QueryResult = await RoleInteractor.AllocateRoleToUserAsync(roleId, userId);
        }
    }
}
