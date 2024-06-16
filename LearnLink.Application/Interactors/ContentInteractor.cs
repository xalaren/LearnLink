using LearnLink.Application.Helpers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Entities.ContentEntities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Interactors
{
    public class ContentInteractor(IUnitOfWork unitOfWork, DirectoryStore directoryStore)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task SaveLessonFileContent(ContentDto contentDto, int lessonId, int sectionId, int contentId)
        {
            try
            {
                if (contentDto is { IsFile: true, FormFile: not null })
                {
                    var fileName = contentDto.FormFile.FileName;

                    await using var stream = contentDto.FormFile.OpenReadStream();
                    
                    if (stream == null || string.IsNullOrWhiteSpace(fileName))
                    {
                        throw new ValidationException("Файл или его название было пустое");
                    }

                    var directory = directoryStore.GetDirectoryPathToLessonSectionContent(lessonId, sectionId, contentId);
                    var contentPath = Path.Combine(directory, fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(contentPath)!);
                    await using var fileStream = new FileStream(contentPath, FileMode.Create);
                    await stream.CopyToAsync(fileStream);
                }
            }
            catch (Exception)
            {
                throw new CustomException("Не удалось сохранить файл контента");
            }
        }

        public void RemoveLessonFileContent(int lessonId, int sectionId, int contentId, string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            var directory = directoryStore.GetDirectoryPathToLessonSectionContent(lessonId, sectionId, contentId);
            var path = Path.Combine(directory, fileName);

            RemoveContent(path);
        }

        public void RemoveObjectiveFileContent(int lessonId, int objectiveId, int contentId, string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            var directory = directoryStore.GetDirectoryPathToLessonObjectiveContent(lessonId, objectiveId, contentId);
            var path = Path.Combine(directory, fileName);

            RemoveContent(path);
        }

        public void RemoveAnswerFileContent(int lessonId, int objectiveId, int answerId, int contentId, string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            var directory = directoryStore.GetDirectoryPathToObjectiveAnswerContent(lessonId, objectiveId, answerId, contentId);
            var path = Path.Combine(directory, fileName);

            RemoveContent(path);
        }


        private void RemoveContent(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return;

            var directory = Path.GetDirectoryName(filePath);

            if (!File.Exists(filePath) || !Directory.Exists(directory))
            {
                return;
            }

            File.Delete(filePath);
            Directory.Delete(directory);
        }
    }
}
