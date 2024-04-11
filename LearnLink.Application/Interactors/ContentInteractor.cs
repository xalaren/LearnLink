using LearnLink.Application.Helpers;
using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;

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

        public async Task<Content> CreateContentWithResult(ContentDto contentDto)
        {
            var contentEntity = contentDto.ToEntity();

            await unitOfWork.Contents.AddAsync(contentEntity);

            if(contentDto.IsFile && contentDto.FormFile != null)
            {
                using(var stream = contentDto.FormFile.OpenReadStream()) 
                {
                    await SaveContentFileAsync(stream, contentEntity);
                }
            }

            return contentEntity;
        }

        public async Task RemoveContentAsyncNoResponse(int contentId)
        {
            var content = await unitOfWork.Contents.FindAsync(contentId);

            if (content == null) return;

            unitOfWork.Contents.Remove(content);
        }

        private async Task SaveContentFileAsync(Stream? stream, Content content)
        {
            if (stream == null || string.IsNullOrWhiteSpace(content.FileName))
            {
                throw new ValidationException("Файл или его название было пустое");
            }

            var directory = directoryStore.GetDirectoryPathToContent(content.Id);
            var contentPath = Path.Combine(directory, DirectoryStore.CONTENT_DIRNAME, content.FileName);

            Directory.CreateDirectory(Path.GetDirectoryName(contentPath)!);

            try
            {
                using (var fileStream = new FileStream(contentPath, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
            catch (Exception)
            {
                throw new CustomException("Не удалось сохранить аватар");
            }
        }
    }
}
