using LearnLink.Application.Interactors;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class SectionsBasePageModel : AuthorizePageModel
    {
        private readonly SectionInteractor sectionInteractor;

        protected SectionInteractor SectionInteractor => sectionInteractor;

        public SectionsBasePageModel(SectionInteractor sectionInteractor)
        {
            this.sectionInteractor = sectionInteractor;
        }
    }
}
