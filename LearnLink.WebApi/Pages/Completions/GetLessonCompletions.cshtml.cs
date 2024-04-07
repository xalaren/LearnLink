using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Completions
{
    public class GetLessonCompletionsModel : CompletionsBasePageModel
    {
        public GetLessonCompletionsModel(CompletionInteractor completionInteractor) : base(completionInteractor) { }

        public Response<LessonCompletionDto[]>? QueryResult { get; set; }

        public LessonCompletionDto[]? LessonCompletions { get; set; }

        public async Task<IActionResult> OnGet(int userId, int moduleId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (userId == 0 || moduleId == 0) return;

                QueryResult = await CompletionInteractor.GetLessonCompletionsOfModuleAsync(userId, moduleId);

                if (QueryResult.Success && QueryResult.Value != null)
                {
                    LessonCompletions = QueryResult.Value;
                }
            });
        }
    }
}
