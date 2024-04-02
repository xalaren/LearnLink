using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Modules
{
    public class DeleteModel : ModulesBasePageModel
    {
        public DeleteModel(ModuleInteractor moduleInteractor) : base(moduleInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int moduleId)
        {
            QueryResult = await ModuleInteractor.RemoveModuleAsync(moduleId);
        }
    }
}
