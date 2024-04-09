using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Lessons
{
    public class ListModel : LessonsBasePageModel
    {
        public ListModel(LessonInteractor lessonInteractor) : base(lessonInteractor) { }

        public Response<LessonDto[]>? QueryResult { get; set; }

        public LessonDto[]? Lessons { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return await AuthRequiredAsync(async () =>
            {
                QueryResult = await LessonInteractor.GetAllLessonsAsync();

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    Lessons = QueryResult.Value;
                }
            });
        }
    }
}
