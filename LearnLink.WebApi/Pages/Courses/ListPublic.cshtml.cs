using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Courses
{
    public class ListPublicModel : CoursesBasePageModel
    {
        public ListPublicModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response<DataPage<CourseDto[]>>? QueryResult { get; set; }

        public CourseDto[]? Courses { get; set; }

        public async Task<IActionResult> OnGet(int pageNumber, int pageSize)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (pageNumber == 0 || pageSize == 0) return;

                QueryResult = await CourseInteractor.GetPublicCoursesAsync(new DataPageHeader(pageNumber, pageSize));

                if (QueryResult.Success && QueryResult.Value != null)
                {
                    Courses = QueryResult.Value.Values;
                }
            });
        }
    }
}
