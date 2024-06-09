using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Sections
{
    public class DeleteModel : SectionsBasePageModel
    {
        public DeleteModel(LessonSectionInteractor sectionInteractor) : base(sectionInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int lessonId, int sectionId)
        {
            QueryResult = await SectionInteractor.RemoveLessonSectionAsync(sectionId, lessonId);
        }
    }
}
