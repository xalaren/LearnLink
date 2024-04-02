using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Modules
{
    public class GetModel : ModulesBasePageModel
    {
        public GetModel(ModuleInteractor moduleInteractor) : base(moduleInteractor) { }

        public Response<ModuleDto?>? QueryResult { get; set; }

        public ModuleDto? FoundModule { get; set; }

        public async Task<IActionResult> OnGet(int moduleId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (moduleId == 0) return;

                QueryResult = await ModuleInteractor.GetModuleAsync(moduleId);

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    FoundModule = QueryResult.Value;
                }
            });
        }
    }
}
