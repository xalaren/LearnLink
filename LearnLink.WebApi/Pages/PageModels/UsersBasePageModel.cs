using LearnLink.Application.Interactors;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class UsersBasePageModel : AuthorizePageModel
    {
        private readonly UserInteractor userInteractor;

        protected UserInteractor UserInteractor => userInteractor;

        public UsersBasePageModel(UserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }
    }
}
