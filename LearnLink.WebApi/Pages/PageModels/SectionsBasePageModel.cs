using LearnLink.Application.Interactors;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class SectionsBasePageModel : AuthorizePageModel
    {
        private readonly LessonSectionInteractor sectionInteractor;

        protected LessonSectionInteractor SectionInteractor => sectionInteractor;

        public SectionsBasePageModel(LessonSectionInteractor sectionInteractor)
        {
            this.sectionInteractor = sectionInteractor;
        }
    }
}
