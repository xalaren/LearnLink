using System.Text.Json.Serialization.Metadata;
using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Courses
{
    public class EditModel : CoursesBasePageModel
    {
        public EditModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response? QueryResult { get; set; }

        public CourseDto? CourseDto { get; set; }


        public async Task<IActionResult> OnGet(int courseId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (courseId == 0) return;

                var result = await CourseInteractor.GetCourseAsync(courseId);

                if(result.Success && result.Value != null)
                {
                    CourseDto = result.Value;
                    return;
                }

                QueryResult = result;
            });
        }

        public async Task OnPost(int userId, int courseId, string title, string? description, string isPublic, string isUnavailable)
        {
            bool isPublicValue = string.IsNullOrWhiteSpace(isPublic) ? false : true;
            bool isUnavailableValue = string.IsNullOrWhiteSpace(isUnavailable) ? false : true;

            var courseDto = new CourseDto(
                Id: courseId,
                Title: title,
                Description: description,
                IsPublic: isPublicValue,
                IsUnavailable: isUnavailableValue);

            QueryResult = await CourseInteractor.UpdateCourseAsync(userId, courseDto);
        }
    }
}
