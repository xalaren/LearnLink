using LearnLink.Application.Interactors;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class RolesBasePageModel : AuthorizePageModel
    {
        private readonly RoleInteractor roleInteractor;

        protected RoleInteractor RoleInteractor => roleInteractor;

        public RolesBasePageModel(RoleInteractor roleInteractor)
        {
            this.roleInteractor = roleInteractor;
        }
    }
}
