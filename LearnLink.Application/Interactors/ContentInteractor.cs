using LearnLink.Application.Transaction;

namespace LearnLink.Application.Interactors
{
    public class ContentInteractor
    {
        private readonly IUnitOfWork unitOfWork;

        public ContentInteractor(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task RemoveContentAsyncNoResponse(int contentId)
        {
            var content = await unitOfWork.Contents.FindAsync(contentId);

            if (content == null) return;

            unitOfWork.Contents.Remove(content);
        }
    }
}
