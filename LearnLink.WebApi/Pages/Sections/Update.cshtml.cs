using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Sections
{
    public class UpdateModel : SectionsBasePageModel
    {
        public UpdateModel(SectionInteractor sectionInteractor) : base(sectionInteractor) { }

        public Response? QueryResult { get; set; }

        public SectionDto? FoundSection { get; set; }

        public async Task<IActionResult> OnGet(int lessonId, int order)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (lessonId == 0 || order == 0) return;

                var result = await SectionInteractor.GetSectionByLessonAndOrderAsync(lessonId, order);

                if (result.Success && result.Value != null)
                {
                    FoundSection = result.Value;
                    return;
                }

                QueryResult = result;
            });
        }

        public async Task OnPost(int lessonId, int sectionId, string? title, string? isText, string? isCodeBlock, string? isFile, string? text, string? lang, IFormFile contentFile)
        {
            bool isTextValue = string.IsNullOrWhiteSpace(isText) ? false : true;
            bool isCodeBlockValue = string.IsNullOrWhiteSpace(isCodeBlock) ? false : true;
            bool isFileValue = string.IsNullOrWhiteSpace(isFile) ? false : true;

            var contentDto = new ContentDto()
            {
                IsText = isTextValue,
                IsCodeBlock = isCodeBlockValue,
                IsFile = isFileValue,
                Text = text,
                FileName = null,
                CodeLanguage = lang,
                FormFile = contentFile
            };

            var sectionDto = new SectionDto()
            {
                Id = sectionId,
                Order = 0,
                Content = contentDto,
                LessonId = lessonId,
                Title = title
            };

            QueryResult = await SectionInteractor.UpdateSectionAsync(lessonId, sectionDto);
        }
    }
}
