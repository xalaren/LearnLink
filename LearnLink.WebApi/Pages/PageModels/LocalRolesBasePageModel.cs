using LearnLink.Application.Interactors;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class LocalRolesBasePageModel : AuthorizePageModel
    {
        private readonly LocalRoleInteractor localRoleInteractor;

        protected LocalRoleInteractor LocalRoleInteractor => localRoleInteractor;

        public LocalRolesBasePageModel(LocalRoleInteractor localRoleInteractor)
        {
            this.localRoleInteractor = localRoleInteractor;
        }
    }
}
