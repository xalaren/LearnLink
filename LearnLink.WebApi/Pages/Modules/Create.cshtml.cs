using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Modules
{
    public class CreateModel : ModulesBasePageModel
    {
        public CreateModel(ModuleInteractor moduleInteractor) : base(moduleInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int courseId, ModuleDto moduleDto)
        {
            QueryResult = await ModuleInteractor.CreateModuleAsync(courseId, moduleDto);
        }
    }
}
