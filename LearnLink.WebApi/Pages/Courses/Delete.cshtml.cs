using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
namespace LearnLink.WebApi.Pages.Courses
{
    public class DeleteModel : CoursesBasePageModel
    {
        public DeleteModel(CourseInteractor courseInteractor) : base(courseInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int userId, int courseId)
        {
            QueryResult = await CourseInteractor.RemoveCourseAsync(userId, courseId);
        }
    }
}
