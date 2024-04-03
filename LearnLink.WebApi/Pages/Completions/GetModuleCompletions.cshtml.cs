using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Completions
{
    public class GetModuleCompletionsModel : CompletionsBasePageModel
    {
        public GetModuleCompletionsModel(CompletionInteractor completionInteractor) : base(completionInteractor) { }

        public Response<ModuleCompletionDto[]>? QueryResult { get; set; }

        public ModuleCompletionDto[]? ModuleCompletions { get; set; }

        public async Task<IActionResult> OnGet(int userId, int courseId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (userId == 0 || courseId == 0) return;

                QueryResult = await CompletionInteractor.GetModuleCompletionsAsync(userId, courseId);

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    ModuleCompletions = QueryResult.Value;
                }
            });
        }
    }
}
