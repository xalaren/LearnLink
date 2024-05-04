using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Modules
{
    public class GetCourseModulesModel : ModulesBasePageModel
    {
        public GetCourseModulesModel(ModuleInteractor moduleInteractor) : base(moduleInteractor) { }

        public Response<ClientModuleDto[]>? QueryResult { get; set; }

        public ClientModuleDto[]? Modules { get; set; }

        public async Task<IActionResult> OnGet(int courseId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (courseId == 0) return;

                QueryResult = await ModuleInteractor.GetCourseModulesAsync(courseId);

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    Modules = QueryResult.Value;
                }
            });
        }
    }
}
