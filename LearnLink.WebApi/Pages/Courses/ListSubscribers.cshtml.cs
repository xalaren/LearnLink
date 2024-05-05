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

        public Response<DataPage<CourseUserDto[]>>? QueryResult { get; set; }

        public CourseUserDto[]? CourseUsers { get; set; }

        public async Task<IActionResult> OnGet(int userId, int courseId, string? searchText, int pageNumber, int pageSize)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (userId == 0 || courseId == 0) return;

                QueryResult = await CourseInteractor.FindParticipantsAsync(userId, courseId, searchText, new DataPageHeader(pageNumber, pageSize));

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    CourseUsers = QueryResult.Value.Values;
                }
            });
        }
    }
}
