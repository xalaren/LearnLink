using LearnLink.Application.Interactors;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class UsersPageModel : AuthorizePageModel
    {
        private readonly UserInteractor userInteractor;

        protected UserInteractor UserInteractor => userInteractor;

        public UsersPageModel(UserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }
    }
}
