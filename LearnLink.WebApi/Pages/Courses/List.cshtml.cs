using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Courses
{
    public class ListModel : CoursesBasePageModel
    {
        public ListModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response<CourseDto[]>? QueryResult { get; set; }

        public CourseDto[]? Courses { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return await AuthRequiredAsync(async () =>
            {
                QueryResult = await CourseInteractor.GetAllAsync();

                if(QueryResult.Success)
                {
                    Courses = QueryResult.Value;
                }
            });
        }
    }
}
