using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.LocalRoles
{
    public class GetAtCourseModel : LocalRolesBasePageModel
    {
        public GetAtCourseModel(LocalRoleInteractor localRoleInteractor) : base(localRoleInteractor) { }

        public Response? QueryResult { get; set; }
        public LocalRoleDto? FoundLocalRole { get; set; }

        public async Task<IActionResult> OnGet(int userId, int courseId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (userId == 0 || courseId == 0) return;

                var response = await LocalRoleInteractor.GetUserLocalRoleAtCourse(courseId, userId);

                if(!response.Success)
                {
                    QueryResult = response;
                    return;
                }

                FoundLocalRole = response.Value;
            });
        }
    }
}
