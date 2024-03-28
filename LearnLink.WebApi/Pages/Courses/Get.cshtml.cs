using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Courses
{
    public class GetModel : CoursesBasePageModel
    {
        public GetModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response<CourseDto?>? QueryResult { get; set; }

        public CourseDto? FoundCourse { get; set; }

        public async Task<IActionResult> OnGet(int courseId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (courseId == 0) return;

                QueryResult = await CourseInteractor.GetCourseAsync(courseId);

                if(QueryResult.Success)
                {
                    FoundCourse = QueryResult.Value;
                }

            });
        }
    }
}
