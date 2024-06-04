using LearnLink.Application.Helpers;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace LearnLink.Application.Mappers
{
    public static class SectionMapper
    {
        public static SectionDto ToDto(this Section sectionEntity)
        {
            var contentDto = new ContentDto()
            {
                IsText = sectionEntity.IsText,
                IsCodeBlock = sectionEntity.IsCodeBlock,
                IsFile = sectionEntity.IsFile,
                Text = sectionEntity.Text,
                CodeLanguage = sectionEntity.CodeLanguage,
                FileName = sectionEntity.FileName,
                FileExtension = sectionEntity.FileExtension,
                FileUrl = sectionEntity.FileName != null ?
                    DirectoryStore.GetRelativeDirectoryUrlToLessonContent(sectionEntity.LessonId, sectionEntity.Id) + sectionEntity.FileName
                     : null
            };

            return new SectionDto()
            {
                Id = sectionEntity.Id,
                LessonId = sectionEntity.LessonId,
                Title = sectionEntity.Title,
                Order = sectionEntity.Order,
                Content = contentDto
            };
        }

        public static Section ToEntity(this SectionDto sectionDto)
        {
            if(sectionDto.Content == null)
            {
                throw new ValidationException("Контент не был заполнен");
            }

            string? fileExt = null;

            if (sectionDto.Content.IsFile && sectionDto.Content.FormFile != null)
            {
                fileExt = Path.GetExtension(sectionDto.Content.FormFile.FileName);
            }

            return new Section()
            {
                LessonId = sectionDto.LessonId,
                Order = sectionDto.Order,
                Title = sectionDto.Title,
                IsText = sectionDto.Content.IsText,
                IsFile = sectionDto.Content.IsFile,
                IsCodeBlock = sectionDto.Content.IsCodeBlock,
                FileName = sectionDto.Content.FormFile?.FileName,
                FileExtension = fileExt,
                Text = sectionDto.Content.Text,
                CodeLanguage = sectionDto.Content.CodeLanguage,
            };
        }

        public static Section ToEntity(this SectionDto sectionDto, Lesson lesson)
        {
            string? fileExt = null;

            if (sectionDto.Content == null)
            {
                throw new ValidationException("Контент не был заполнен");
            }

            if (sectionDto.Content.IsFile && sectionDto.Content.FormFile != null)
            {
                fileExt = Path.GetExtension(sectionDto.Content.FormFile.FileName);
            }

            return new Section()
            {
                Lesson = lesson,
                Order = sectionDto.Order,
                Title = sectionDto.Title,
                IsText = sectionDto.Content.IsText,
                IsFile = sectionDto.Content.IsFile,
                IsCodeBlock = sectionDto.Content.IsCodeBlock,
                FileName = sectionDto.Content.FormFile?.FileName,
                FileExtension = fileExt,
                Text = sectionDto.Content.Text,
                CodeLanguage = sectionDto.Content.CodeLanguage
            };
        }

        public static Section Assign(this Section sectionEntity, SectionDto sectionDto)
        {
            if (sectionDto.Content == null)
            {
                throw new ValidationException("Контент не был заполнен");
            }
            sectionEntity.Title = sectionDto.Title;
            sectionEntity.IsText = sectionDto.Content.IsText;
            sectionEntity.IsCodeBlock = sectionDto.Content.IsCodeBlock;
            sectionEntity.IsFile = sectionDto.Content.IsFile;
            sectionEntity.Text = sectionDto.Content.Text;
            sectionEntity.CodeLanguage = sectionDto.Content.CodeLanguage;

            if (sectionDto.Content.IsFile && sectionDto.Content.FormFile != null)
            {
                sectionEntity.FileName = sectionDto.Content.FormFile.FileName;
                sectionEntity.FileExtension = Path.GetExtension(sectionDto.Content.FormFile.FileName);
            }

            return sectionEntity;
        }

        public static SectionDto ToSectionDto(this SectionFileContentDto sectionFileContentDto)
        {
            string fileExt = Path.GetExtension(sectionFileContentDto.FormFile.FileName);
            string fileName = sectionFileContentDto.FormFile.FileName;

            return new SectionDto()
            {
                Id = sectionFileContentDto.Id,
                LessonId = sectionFileContentDto.LessonId,
                Order = sectionFileContentDto.Order,
                Title = sectionFileContentDto.Title,
                Content = new ContentDto()
                {
                    IsText = false,
                    IsCodeBlock = false,
                    IsFile = true,
                    FormFile = sectionFileContentDto.FormFile,
                    FileName = fileName,
                    FileExtension = fileExt
                }
            };
        }

        public static SectionDto ToSectionDto(this SectionTextContentDto sectionTextContentDto)
        {
            return new SectionDto()
            {
                Id = sectionTextContentDto.Id,
                LessonId = sectionTextContentDto.LessonId,
                Order = sectionTextContentDto.Order,
                Title = sectionTextContentDto.Title,
                Content = new ContentDto()
                {
                    IsText = true,
                    IsCodeBlock = false,
                    IsFile = false,
                    Text = sectionTextContentDto.Text
                }
            };
        }
        public static SectionDto ToSectionDto(this SectionCodeContentDto sectionCodeContentDto)
        {

            return new SectionDto()
            {
                Id = sectionCodeContentDto.Id,
                LessonId = sectionCodeContentDto.LessonId,
                Order = sectionCodeContentDto.Order,
                Title = sectionCodeContentDto.Title,
                Content = new ContentDto()
                {
                    IsText = false,
                    IsCodeBlock = true,
                    IsFile = false,
                    Text = sectionCodeContentDto.Code,
                    CodeLanguage = sectionCodeContentDto.CodeLanguage
                }
            };
        }
    }
}
