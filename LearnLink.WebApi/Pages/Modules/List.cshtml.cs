using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Modules
{
    public class ListModel : ModulesBasePageModel
    {
        public ListModel(ModuleInteractor moduleInteractor) : base(moduleInteractor) { }

        public Response<ModuleDto[]>? QueryResult { get; set; }

        public ModuleDto[]? Modules { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return await AuthRequiredAsync(async () =>
            {
                QueryResult = await ModuleInteractor.GetAllModulesAsync();

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    Modules = QueryResult.Value;
                }
            });
        }
    }
}
