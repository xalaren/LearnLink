using LearnLink.Application.Helpers;
using LearnLink.Core.Entities;
using LearnLink.Core.Entities.ContentEntities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class SectionMapper
    {
        public static SectionDto ToDto(this Section sectionEntity, int lessonId = 0)
        {
            bool isText = sectionEntity.TextContent != null;
            bool isCodeBlock = sectionEntity.CodeContent != null;
            bool isFile = sectionEntity.FileContent != null;

            string? text = null;

            if (sectionEntity.TextContent == null && sectionEntity.CodeContent == null && sectionEntity.FileContent == null)
            {
                throw new ValidationException("Контент был пуст");
            }

            string fileUrl = "";

            if (sectionEntity.FileContent != null && sectionEntity.FileContentId != null)
            {
                fileUrl = Path.Combine
                (
                    DirectoryStore.GetRelativeDirectoryUrlToLessonSectionContent(lessonId, sectionEntity.Id, sectionEntity.FileContent.Id),
                    sectionEntity.FileContent.FileName
                );
            }

            if (sectionEntity.TextContent != null)
            {
                text = sectionEntity.TextContent.Text;
            }
            else if (sectionEntity.CodeContent != null)
            {
                text = sectionEntity.CodeContent.CodeText;
            }



            var contentDto = new ContentDto()
            {
                IsText = isText,
                IsCodeBlock = isCodeBlock,
                IsFile = isFile,
                Text = text,
                CodeLanguage = sectionEntity.CodeContent?.CodeLanguage,
                FileName = sectionEntity.FileContent?.FileName,
                FileExtension = sectionEntity.FileContent?.FileExtension,
                FileUrl = fileUrl,
            };

            return new SectionDto()
            {
                Id = sectionEntity.Id,
                Title = sectionEntity.Title,
                Order = sectionEntity.Order,
                Content = contentDto
            };
        }

        public static Section ToEntity(this SectionDto sectionDto)
        {
            if (sectionDto.Content == null)
            {
                throw new ValidationException("Контент не был заполнен");
            }

            if (sectionDto.Content.IsText)
            {
                return new Section()
                {
                    Id = sectionDto.Id,
                    Order = sectionDto.Order,
                    Title = sectionDto.Title,
                    TextContent = new TextContent()
                    {
                        Id = 0,
                        Text = sectionDto.Content.Text!
                    }
                };
            }

            if (sectionDto.Content.IsFile)
            {
                string fileExt = "";
                string fileName = "";

                if (sectionDto.Content.FormFile != null)
                {
                    fileName = sectionDto.Content.FormFile.FileName;
                    fileExt = Path.GetExtension(sectionDto.Content.FormFile.FileName);
                }

                return new Section()
                {
                    Id = sectionDto.Id,
                    Order = sectionDto.Order,
                    Title = sectionDto.Title,
                    FileContent = new FileContent()
                    {
                        Id = 0,
                        FileExtension = fileExt,
                        FileName = fileName
                    }
                };
            }

            if (sectionDto.Content.IsCodeBlock)
            {
                return new Section()
                {
                    Id = sectionDto.Id,
                    Order = sectionDto.Order,
                    Title = sectionDto.Title,
                    CodeContent = new CodeContent()
                    {
                        Id = 0,
                        CodeLanguage = sectionDto.Content.CodeLanguage!,
                        CodeText = sectionDto.Content.Text!
                    }
                };
            }

            return new Section()
            {
                Id = sectionDto.Id,
                Order = sectionDto.Order,
                Title = sectionDto.Title
            };
        }

        public static Section Assign(this Section sectionEntity, SectionDto sectionDto)
        {
            sectionEntity.Title = sectionDto.Title;

            if (sectionDto.Content is { IsText: true })
            {
                if (sectionEntity.TextContent == null)
                {
                    throw new ValidationException("Текстовый контент не объявляен для данного раздела");
                }
                sectionEntity.TextContent.Text = sectionDto.Content.Text!;
            }
            else if (sectionDto.Content is { IsFile: true })
            {
                string fileExt = "";
                string fileName = "";

                if (sectionEntity.FileContent == null)
                {
                    throw new ValidationException("Файловый контент не объявляен для данного раздела");
                }

                if (sectionDto.Content.FormFile != null)
                {
                    fileName = sectionDto.Content.FormFile.FileName;
                    fileExt = Path.GetExtension(sectionDto.Content.FormFile.FileName);
                }

                sectionEntity.FileContent.FileName = fileName;
                sectionEntity.FileContent.FileExtension = fileExt;
            }
            else if (sectionDto.Content is { IsCodeBlock: true })
            {
                if (sectionEntity.CodeContent == null)
                {
                    throw new ValidationException("Контент для кода не объявляен для данного раздела");
                }

                sectionEntity.CodeContent.CodeText = sectionDto.Content.Text!;
                sectionEntity.CodeContent.CodeLanguage = sectionDto.Content.CodeLanguage!;
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
