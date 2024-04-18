using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Sections
{
    public class ChangeOrderModel : SectionsBasePageModel
    {
        public ChangeOrderModel(SectionInteractor sectionInteractor) : base(sectionInteractor) { }

        public Response? QueryResult { get; set; }

        public SectionDto? FoundSection { get; set; }


        public async Task<IActionResult> OnGet(int lessonId, int order)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (lessonId == 0 || order == 0) return;

                var result = await SectionInteractor.GetSectionByLessonAndOrder(lessonId, order);

                if(result.Success && result.Value != null)
                {
                    FoundSection = result.Value;
                    return;
                }

                QueryResult = result;
            });
        }

        public async Task OnPostIncrease(int sectionId, int lessonId)
        {
            QueryResult = await SectionInteractor.ChangeOrder(sectionId, lessonId, true);
        }

        public async Task OnPostDecrease(int sectionId, int lessonId)
        {
            QueryResult = await SectionInteractor.ChangeOrder(sectionId, lessonId, false);
        }
    }
}
