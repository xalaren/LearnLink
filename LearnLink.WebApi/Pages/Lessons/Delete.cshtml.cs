using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Lessons
{
    public class RemoveModel : LessonsBasePageModel
    {
        public RemoveModel(LessonInteractor lessonInteractor) : base(lessonInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int lessonId)
        {
            QueryResult = await LessonInteractor.RemoveLessonAsync(lessonId);
        }
    }
}
