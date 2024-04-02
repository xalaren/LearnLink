using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Courses
{
    public class CreateModel : CoursesBasePageModel
    {
        public CreateModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int userId, string title, string description, string isPublic, string isUnavailable)
        {
            bool isPublicValue = string.IsNullOrWhiteSpace(isPublic) ? false : true;
            bool isUnavailableValue = string.IsNullOrWhiteSpace(isUnavailable) ? false : true;

            var courseDto = new CourseDto(
                Id: 0,
                Title: title,
                Description: description,
                IsPublic: isPublicValue,
                IsUnavailable: isUnavailableValue);

            QueryResult = await CourseInteractor.CreateCourseAsync(userId, courseDto); 
        }
    }
}
