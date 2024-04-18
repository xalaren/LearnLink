using LearnLink.Application.Helpers;
using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class SectionMapper
    {
        public static SectionDto ToDto(this Section sectionEntity)
        {
            var contentDto = new ContentDto
            (
                IsText: sectionEntity.IsText,
                IsCodeBlock: sectionEntity.IsCodeBlock,
                IsFile: sectionEntity.IsFile,
                Text: sectionEntity.Text,
                FileName: sectionEntity.FileName,
                FileUrl: sectionEntity.FileName != null ? 
                    DirectoryStore.GetRelativeDirectoryUrlToLessonContent(sectionEntity.LessonId, sectionEntity.Id) +  sectionEntity.FileName
                    : null
            );

            return new SectionDto
            (
                Id: sectionEntity.Id,
                LessonId: sectionEntity.LessonId,
                Title: sectionEntity.Title,
                Order: sectionEntity.Order,
                ContentDto: contentDto
            );
        }

        public static Section ToEntity(this SectionDto sectionDto)
        {
            return new Section()
            {
                LessonId = sectionDto.LessonId,
                Order = sectionDto.Order,
                Title = sectionDto.Title,
                IsText = sectionDto.ContentDto.IsText,
                IsFile = sectionDto.ContentDto.IsFile,
                IsCodeBlock = sectionDto.ContentDto.IsCodeBlock,
                FileName = sectionDto.ContentDto.FormFile?.FileName,
                Text = sectionDto.ContentDto.Text
            };
        }

        public static Section ToEntity(this SectionDto sectionDto, Lesson lesson)
        {
            return new Section()
            {
                Lesson = lesson,
                Order = sectionDto.Order,
                Title = sectionDto.Title,
                IsText = sectionDto.ContentDto.IsText,
                IsFile = sectionDto.ContentDto.IsFile,
                IsCodeBlock = sectionDto.ContentDto.IsCodeBlock,
                FileName = sectionDto.ContentDto.FormFile?.FileName,
                Text = sectionDto.ContentDto.Text
            };
        }

        public static Section Assign(this Section sectionEntity, SectionDto sectionDto)
        {
            sectionEntity.Title = sectionDto.Title ?? sectionEntity.Title;
            sectionEntity.Order = sectionDto.Order;
            sectionEntity.IsText = sectionDto.ContentDto.IsText;
            sectionEntity.IsCodeBlock = sectionDto.ContentDto.IsCodeBlock;
            sectionEntity.IsFile = sectionDto.ContentDto.IsFile;
            sectionEntity.Text = sectionDto.ContentDto.Text;
            sectionEntity.FileName = sectionDto.ContentDto.FileName;

            return sectionEntity;
        }
    }
}
