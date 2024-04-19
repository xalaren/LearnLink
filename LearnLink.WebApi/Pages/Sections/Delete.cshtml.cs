using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Sections
{
    public class DeleteModel : SectionsBasePageModel
    {
        public DeleteModel(SectionInteractor sectionInteractor) : base(sectionInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int sectionId)
        {
            QueryResult = await SectionInteractor.RemoveSectionAsync(sectionId);
        }
    }
}
