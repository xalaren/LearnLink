using LearnLink.Application.Helpers;
using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class ContentMapper
    {
        public static Content ToEntity(this ContentDto contentDto)
        {
            return new Content()
            {
                Id = contentDto.Id,
                IsText = contentDto.IsText,
                IsCodeBlock = contentDto.IsCodeBlock,
                IsFile = contentDto.IsFile,
                Text = contentDto.Text,
                FileName = contentDto.FileName,
            };
        }

        public static ContentDto ToDto(this Content contentEntity)
        {
            return new ContentDto
                (
                    Id: contentEntity.Id,
                    IsText: contentEntity.IsText,
                    IsCodeBlock: contentEntity.IsCodeBlock,
                    IsFile: contentEntity.IsFile,
                    Text: contentEntity.Text,
                    FileName: contentEntity.FileName,
                    FileUrl: contentEntity.FileName != null ?
                        DirectoryStore.GetRelativeDirectoryUrlToContent(contentEntity.Id) + contentEntity.FileName : null
                );
        }
    }
}
