using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Sections
{
    public class ListModel : SectionsBasePageModel
    {
        public ListModel(SectionInteractor sectionInteractor) : base(sectionInteractor) { }

        public Response<SectionDto[]>? QueryResult { get; set; }

        public SectionDto[]? Sections { get; set; }

        public async Task<IActionResult> OnGet(int lessonId)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (lessonId == 0) return;

                QueryResult = await SectionInteractor.GetLessonSections(lessonId);

                if(QueryResult.Success && QueryResult.Value != null)
                {
                    Sections = QueryResult.Value;
                }
            });
        }
    }
}
