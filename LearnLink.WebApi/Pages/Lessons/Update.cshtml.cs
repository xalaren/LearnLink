using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Lessons
{
    public class UpdateModel : LessonsBasePageModel
    {
        public UpdateModel(LessonInteractor lessonInteractor) : base(lessonInteractor) { }

        public Response? QueryResult { get; set; }

        public LessonDto? FoundLesson{ get; set; }

        public async Task<IActionResult> OnGet(int lessonId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (lessonId == 0) return;

                var result = await LessonInteractor.GetLessonAsync(lessonId);

                if (result.Success && result.Value != null)
                {
                    FoundLesson = result.Value;
                    return;
                }

                QueryResult = result;
            });
        }

        public async Task OnPost(LessonDto lessonDto)
        {
            QueryResult = await LessonInteractor.UpdateLessonAsync(lessonDto);
        }
    }
}
