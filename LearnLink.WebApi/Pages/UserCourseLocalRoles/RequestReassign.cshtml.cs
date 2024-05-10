using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.UserCourseLocalRoles
{
    public class RequestReassignModel : UserCourseLocalRolesBasePageModel
    {
        public RequestReassignModel(UserCourseLocalRolesInteractor userCourseLocalRolesInteractor) : base(userCourseLocalRolesInteractor) {}

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int requesterUserId, int targetUserId, int courseId, int localRoleId)
        {
            QueryResult = await UserCourseLocalRolesInteractor.RequestReassignUserRoleAsync(requesterUserId, targetUserId, courseId, localRoleId);
        }
    }
}
