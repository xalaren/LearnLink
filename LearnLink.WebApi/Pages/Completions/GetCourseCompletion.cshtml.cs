using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Completions
{
    public class GetCourseCompletionModel : CompletionsBasePageModel
    {
        public GetCourseCompletionModel(CompletionInteractor completionInteractor) : base(completionInteractor) { }

        public Response<CourseCompletionDto>? QueryResult { get; set; }

        public CourseCompletionDto? FoundCourseCompletion { get; set; }

        public async Task<IActionResult> OnGet(int userId, int courseId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (userId == 0 || courseId == 0) return;

                QueryResult = await CompletionInteractor.GetCourseCompletion(userId, courseId);

                if (QueryResult.Success && QueryResult.Value != null)
                {
                    FoundCourseCompletion = QueryResult.Value;
                }
            });
        }
    }
}
