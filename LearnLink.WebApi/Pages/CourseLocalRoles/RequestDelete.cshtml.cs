using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.CourseLocalRoles
{
    public class RequestDeleteModel : CourseLocalRolesBasePageModel
    {
        public RequestDeleteModel(CourseLocalRoleInteractor courseLocalRoleInteractor) : base(courseLocalRoleInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int requesterUserId, int courseId, int localRoleId)
        {
            QueryResult = await CourseLocalRoleInteractor.RequestRemoveAsync(requesterUserId, courseId, localRoleId);
        }
    }
}
