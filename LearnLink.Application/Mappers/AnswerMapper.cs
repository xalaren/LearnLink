using System.Runtime.CompilerServices;
using LearnLink.Application.Helpers;
using LearnLink.Core.Entities;
using LearnLink.Core.Entities.ContentEntities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class AnswerMapper
    {
        public static Answer ToEntity(this AnswerDto answerDto)
        {
            FileContent? fileContent = null;

            if (answerDto.FormFile != null)
            {
                fileContent = new FileContent()
                {
                    Id = 0,
                    FileExtension = Path.GetExtension(answerDto.FormFile.FileName),
                    FileName = answerDto.FormFile.FileName
                };
            }

            TextContent? textContent = null;

            if (!string.IsNullOrWhiteSpace(answerDto.Text))
            {
                textContent = new TextContent()
                {
                    Id = 0,
                    Text = answerDto.Text
                };
            }

            if(fileContent == null && textContent == null)
            {
                throw new ValidationException("Ответ не был заполнен");
            }

            return new Answer()
            {
                Id = answerDto.Id,
                ObjectiveId = answerDto.ObjectiveId,
                UserId = answerDto.UserId,
                FileContent = fileContent,
                TextContent = textContent,
                UploadDate = DateTime.UtcNow
            };
        }

        public static AnswerDto ToDto(this Answer answerEntity, int lessonId)
        {
            string? fileUrl = null;
            string? fileName = null;
            string? fileExt = null;

            if (answerEntity.FileContent != null)
            {
                int fileContentId = answerEntity.FileContentId ?? 0;
                fileName = answerEntity.FileContent.FileName;
                fileExt = Path.GetExtension(fileName);
                fileUrl = DirectoryStore.GetRelativeDirectoryUrlToLessonObjectiveAnswerContent
                (
                    lessonId,
                    answerEntity.ObjectiveId,
                    answerEntity.Id,
                    fileContentId
                );
            }

            return new AnswerDto()
            {
                Id = answerEntity.Id,
                UserId = answerEntity.UserId,
                User = answerEntity.User.ToDto(),
                ObjectiveId = answerEntity.ObjectiveId,
                FileName = fileName,
                FileExtension = fileExt,
                FileUrl = fileUrl
            };
        }

        public static Answer Assign(this Answer answerEntity, AnswerDto answerDto)
        {
            answerEntity.UploadDate = DateTime.UtcNow;

            if(answerEntity.TextContent != null)
            {
                if(string.IsNullOrWhiteSpace(answerDto.Text))
                {
                    answerEntity.TextContent = null;
                }
                else
                {
                    answerEntity.TextContent.Text = answerDto.Text;
                }
            }

            if (answerDto.FormFile != null)
            {
                answerEntity.FileContent = new FileContent()
                {
                    Id = 0,
                    FileExtension = Path.GetExtension(answerDto.FormFile.FileName),
                    FileName = answerDto.FormFile.FileName
                };
            }

            return answerEntity;
        }
    }
}
