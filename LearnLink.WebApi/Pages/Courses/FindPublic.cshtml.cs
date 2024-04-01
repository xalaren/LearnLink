using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Courses
{
    public class FindPublicModel : CoursesBasePageModel
    {
        public FindPublicModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response<DataPage<CourseDto[]>>? QueryResult { get; set; }

        public CourseDto[]? Courses { get; set; }

        public async Task<IActionResult> OnGet(string searchText, int pageNumber, int pageSize)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(searchText)) return;

                QueryResult = await CourseInteractor.FindCoursesByTitle(searchText, new DataPageHeader(pageNumber, pageSize));

                if(QueryResult.Success && QueryResult.Value != null && QueryResult.Value.Values != null)
                {
                    Courses = QueryResult.Value.Values;
                }
            });
        }
    }
}
