using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace LearnLink.WebApi.Pages.Sections
{
    public class CreateModel : SectionsBasePageModel
    {
        public CreateModel(SectionInteractor sectionInteractor) : base(sectionInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int lessonId, string? title, string? isText, string? isCodeBlock, string? isFile, string? text, IFormFile contentFile)
        {
            bool isTextValue = string.IsNullOrWhiteSpace(isText) ? false : true;
            bool isCodeBlockValue = string.IsNullOrWhiteSpace(isCodeBlock) ? false : true;
            bool isFileValue = string.IsNullOrWhiteSpace(isFile) ? false : true;

            var contentDto = new ContentDto
            (
                isTextValue,
                isCodeBlockValue,
                isFileValue,
                text,
                null,
                contentFile
            );

            var sectionDto = new SectionDto
            (
                Id: 0,
                Order: 0,
                ContentDto: contentDto,
                LessonId: lessonId,
                Title: title
            );

            QueryResult = await SectionInteractor.CreateSectionAsync(lessonId, sectionDto);
        }
    }
}
