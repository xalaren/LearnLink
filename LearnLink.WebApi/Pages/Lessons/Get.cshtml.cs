using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Lessons
{
    public class GetModel : LessonsBasePageModel
    {
        public GetModel(LessonInteractor lessonInteractor) : base(lessonInteractor) { }

        public Response<LessonDto>? QueryResult { get; set; }

        public LessonDto? FoundLesson { get; set; }

        public async Task<IActionResult> OnGet(int lessonId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (lessonId == 0) return;

                QueryResult = await LessonInteractor.GetLessonAsync(lessonId);

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    FoundLesson = QueryResult.Value;
                }
            });
        }
    }
}
