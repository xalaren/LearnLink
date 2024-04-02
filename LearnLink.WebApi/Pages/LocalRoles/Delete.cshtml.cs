using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.LocalRoles
{
    public class DeleteModel : LocalRolesBasePageModel
    {
        public DeleteModel(LocalRoleInteractor localRoleInteractor) : base(localRoleInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int localRoleId)
        {
            QueryResult = await LocalRoleInteractor.RemoveLocalRoleAsync(localRoleId);
        }

    }
}
