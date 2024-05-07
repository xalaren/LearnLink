using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.LocalRoles
{
    public class ReassignModel : LocalRolesBasePageModel
    {
        public ReassignModel(LocalRoleInteractor localRoleInteractor) : base(localRoleInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int requesterUserId, int targetUserId, int courseId, int localRoleId)
        {
            QueryResult = await LocalRoleInteractor.ReassignUserRoleAsync(requesterUserId, targetUserId, courseId, localRoleId);
        }
    }
}
