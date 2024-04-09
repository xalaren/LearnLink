using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Lessons
{
    public class CreateModel : LessonsBasePageModel
    {
        public CreateModel(LessonInteractor lessonInteractor) : base(lessonInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int courseId, int moduleId, LessonDto lessonDto)
        {
            QueryResult = await LessonInteractor.CreateLessonAsync(courseId, moduleId, lessonDto);
        }
    }
}
