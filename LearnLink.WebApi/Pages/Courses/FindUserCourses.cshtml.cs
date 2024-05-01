using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Courses
{
    public class FindUserCoursesModel : CoursesBasePageModel
    {
        public FindUserCoursesModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response<DataPage<CourseDto[]>>? QueryResult { get; set; }

        public CourseDto[]? Courses { get; set; }

        public async Task<IActionResult> OnGet(int userId, string searchText, string subscriptionString, string unavailableString, int pageNumber, int pageSize)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (userId == 0) return;

                var isSubscription = string.IsNullOrWhiteSpace(subscriptionString) ? false : true;
                var isUnavailable = string.IsNullOrWhiteSpace(unavailableString) ? false : true;

                QueryResult = await CourseInteractor.FindCoursesByTitleInUserCourses(userId, searchText, isSubscription, isUnavailable, new DataPageHeader(pageNumber, pageSize));

                if (QueryResult.Success && QueryResult.Value != null)
                {
                    Courses = QueryResult.Value.Values;
                }
            });
        }
    }
}
