using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace LearnLink.WebApi.Pages.Courses
{
    public class ListUserCoursesModel : CoursesBasePageModel
    {
        public ListUserCoursesModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public CourseDto[]? Courses { get; set; }

        public Response<CourseDto[]>? QueryResult { get; set; }

        public async Task<IActionResult> OnGet(int userId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (userId == 0) return;

                QueryResult = await CourseInteractor.GetCoursesCreatedByUserAsync(userId);

                if (QueryResult.Success)
                {
                    Courses = QueryResult.Value;
                }
            });
        }
    }
}
