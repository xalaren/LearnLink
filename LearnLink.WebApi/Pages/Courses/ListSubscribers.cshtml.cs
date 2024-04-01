using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Courses
{
    public class ListSubscribersModel : CoursesBasePageModel
    {
        public ListSubscribersModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response<CourseUserDto[]>? QueryResult { get; set; }

        public CourseUserDto[]? CourseUsers { get; set; }

        public async Task<IActionResult> OnGet(int userId, int courseId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (userId == 0 || courseId == 0) return;

                QueryResult = await CourseInteractor.GetSubscribers(userId, courseId);

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    CourseUsers = QueryResult.Value;
                }
            });
        }
    }
}
