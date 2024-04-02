using LearnLink.Application.Interactors;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class ModulesBasePageModel : AuthorizePageModel
    {
        private readonly ModuleInteractor moduleInteractor;

        protected ModuleInteractor ModuleInteractor => moduleInteractor;

        public ModulesBasePageModel(ModuleInteractor moduleInteractor)
        {
            this.moduleInteractor = moduleInteractor;
        }
    }
}
