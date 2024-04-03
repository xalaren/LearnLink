using LearnLink.Application.Interactors;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class CompletionsBasePageModel : AuthorizePageModel
    {
        private readonly CompletionInteractor completionInteractor;

        protected CompletionInteractor CompletionInteractor => completionInteractor;

        public CompletionsBasePageModel(CompletionInteractor completionInteractor)
        {
            this.completionInteractor = completionInteractor;
        }
    }
}
