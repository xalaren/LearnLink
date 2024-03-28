using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Courses
{
    public class GetAnyModel : CoursesBasePageModel
    {
        public GetAnyModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response<CourseDto?>? QueryResult { get; set; }

        public CourseDto? FoundCourse { get; set; }

        public async Task<IActionResult> OnGet(int userId, int courseId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (userId == 0 || courseId == 0) return;

                QueryResult = await CourseInteractor.GetAnyCourseAsync(userId, courseId);

                if (QueryResult.Success)
                {
                    FoundCourse = QueryResult.Value;
                }
            });
        }
    }
}
