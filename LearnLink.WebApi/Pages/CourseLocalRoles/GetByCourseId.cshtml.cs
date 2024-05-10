using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.CourseLocalRoles
{
    public class GetByCourseIdModel : CourseLocalRolesBasePageModel
    {
        public GetByCourseIdModel(CourseLocalRoleInteractor courseLocalRoleInteractor) : base(courseLocalRoleInteractor) { }

        public Response? QueryResult { get; set; }
        public LocalRoleDto[]? LocalRoles { get; set; }

        public async Task<IActionResult> OnGet(int courseId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (courseId == 0) return;

                var response = await CourseLocalRoleInteractor.GetLocalRolesAtCourseAsync(courseId);

                if (response.Success && response.Value != null)
                {
                    LocalRoles = response.Value;
                    return;
                }

                QueryResult = response;
            });
        }
    }
}
