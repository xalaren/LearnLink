using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Modules
{
    public class UpdateModel : ModulesBasePageModel
    {
        public UpdateModel(ModuleInteractor moduleInteractor) : base(moduleInteractor) { }

        public Response? QueryResult { get; set; }

        public ModuleDto? FoundModule { get; set; }

        public async Task<IActionResult> OnGet(int moduleId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (moduleId == 0) return;

                var result = await ModuleInteractor.GetModuleAsync(moduleId);

                if(result.Success && result.Value != null)
                {
                    FoundModule = result.Value;
                    return;
                }

                QueryResult = result;
            });
        }

        public async Task OnPost(ModuleDto moduleDto)
        {
            QueryResult = await ModuleInteractor.UpdateModuleAsync(moduleDto);
        }
    }
}
