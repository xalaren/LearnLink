using System.Diagnostics;
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
            if(objectiveDto.FormFile == null)
            {
                throw new ValidationException("Файл не был добавлен");
            }

            return new Objective()
            {
                Id = objectiveDto.Id,
                Title = objectiveDto.Title,
                Text = objectiveDto.Text,
                FileContent = new FileContent()
                {
                    Id = 0,
                    FileExtension = Path.GetExtension(objectiveDto.FormFile.FileName),
                    FileName = objectiveDto.FormFile.FileName
                }
            };
        }

        public static ObjectiveDto ToDto(this Objective objectiveEntity, int lessonId)
        {
            return new ObjectiveDto()
            {
                Id = objectiveEntity.Id,
                Text = objectiveEntity.Text,
                Title = objectiveEntity.Title,
                FileUrl = DirectoryStore.GetRelativeDirectoryUrlToLessonObjectiveContent(lessonId, objectiveEntity.Id, objectiveEntity.FileContent.Id)
            };
        }
    }
}
