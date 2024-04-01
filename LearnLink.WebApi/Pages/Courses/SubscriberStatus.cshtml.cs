using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Courses
{
    public class SubscriberStatusModel : CoursesBasePageModel
    {
        public SubscriberStatusModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response<bool>? QueryResult { get; set; }

        public async Task<IActionResult> OnGet(int userId, int courseId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (userId == 0 || courseId == 0) return;

                QueryResult = await CourseInteractor.IsSubscriber(userId, courseId);
            });
        }
    }
}
