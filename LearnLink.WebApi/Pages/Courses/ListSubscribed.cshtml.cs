using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Courses
{
    public class ListSubscribedModel : CoursesBasePageModel
    {
        public ListSubscribedModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response<CourseDto[]>? QueryResult { get; set; }

        public CourseDto[]? Courses { get; set; }

        public async Task<IActionResult> OnGet(int userId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (userId == 0) return;

                QueryResult = await CourseInteractor.GetSubscribedCoursesAsync(userId);

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    Courses = QueryResult.Value;
                }
            });
        }
    }
}
