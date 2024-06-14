using LearnLink.Application.Helpers;
using LearnLink.Core.Entities;
using LearnLink.Core.Entities.ContentEntities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class ObjectiveMapper
    {
        public static Objective ToEntity(this ObjectiveDto objectiveDto)
        {
            FileContent? fileContent = null;

            if (objectiveDto.FormFile != null)
            {
                fileContent = new FileContent()
                {
                    Id = 0,
                    FileExtension = Path.GetExtension(objectiveDto.FormFile.FileName),
                    FileName = objectiveDto.FormFile.FileName
                };
            }

            return new Objective()
            {
                Id = objectiveDto.Id,
                Title = objectiveDto.Title,
                Text = objectiveDto.Text,
                FileContent = fileContent
            };
        }

        public static ObjectiveDto ToDto(this Objective objectiveEntity, int lessonId)
        {
            int fileContentId = 0;
            string? fileUrl = null;
            string? fileName = null;
            string? fileExt = null;

            if (objectiveEntity.FileContent != null)
            {
                fileContentId = objectiveEntity.FileContentId ?? 0;
                fileName = objectiveEntity.FileContent.FileName;
                fileExt = Path.GetExtension(fileName);
                fileUrl = DirectoryStore.GetRelativeDirectoryUrlToLessonObjectiveContent
                (
                    lessonId,
                    objectiveEntity.Id,
                    fileContentId
                );
            }

            return new ObjectiveDto()
            {
                Id = objectiveEntity.Id,
                Text = objectiveEntity.Text,
                Title = objectiveEntity.Title,
                FileContentId = fileContentId,
                FileName = fileName,
                FileExtension = fileExt,
                FileUrl = fileUrl
            };
        }

        public static Objective Assign(this Objective objectiveEntity, ObjectiveDto objectiveDto)
        {
            objectiveEntity.Title = !string.IsNullOrWhiteSpace(objectiveDto.Title) ? objectiveDto.Title : objectiveEntity.Title;
            objectiveEntity.Text = !string.IsNullOrWhiteSpace(objectiveDto.Text) ? objectiveDto.Text : objectiveEntity.Text;

            if (objectiveDto.FormFile != null)
            {
                objectiveEntity.FileContent = new FileContent()
                {
                    Id = 0,
                    FileExtension = Path.GetExtension(objectiveDto.FormFile.FileName),
                    FileName = objectiveDto.FormFile.FileName
                };
            }

            return objectiveEntity;
        }
    }
}
