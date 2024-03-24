using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Roles
{
    public class DeleteModel : RolesBasePageModel
    {
        public DeleteModel(RoleInteractor roleInteractor) : base(roleInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int roleId)
        {
            QueryResult = await RoleInteractor.RemoveRoleAsync(roleId);
        }
    }
}
