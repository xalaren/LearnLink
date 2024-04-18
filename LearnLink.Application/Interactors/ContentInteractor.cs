using LearnLink.Application.Helpers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Interactors
{
    public class ContentInteractor
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly DirectoryStore directoryStore;

        public ContentInteractor(IUnitOfWork unitOfWork, DirectoryStore directoryStore)
        {
            this.unitOfWork = unitOfWork;
            this.directoryStore = directoryStore;
        }

        public async Task SaveLessonContentAsync(ContentDto contentDto, int lessonId, int sectionId)
        {
            try
            {
                if (contentDto.IsFile && contentDto.FormFile != null)
                {
                    var fileName = contentDto.FormFile.FileName;

                    using (var stream = contentDto.FormFile.OpenReadStream())
                    {
                        if (stream == null || string.IsNullOrWhiteSpace(fileName))
                        {
                            throw new ValidationException("Файл или его название было пустое");
                        }

                        var directory = directoryStore.GetDirectoryPathToLessonContents(lessonId, sectionId);
                        var contentPath = Path.Combine(directory, fileName);

                        Directory.CreateDirectory(Path.GetDirectoryName(contentPath)!);
                        using (var fileStream = new FileStream(contentPath, FileMode.Create))
                        {
                            await stream.CopyToAsync(fileStream);
                        }

                    }
                }
            }
            catch(Exception)
            {
                throw new CustomException("Не удалось сохранить файл контента");
            }
        }

        public void RemoveLessonContent(int lessonId, int sectionId, string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            var directory = directoryStore.GetDirectoryPathToLessonContents(lessonId, sectionId);
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
