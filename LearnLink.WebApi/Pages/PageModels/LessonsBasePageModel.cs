using LearnLink.Application.Interactors;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class LessonsBasePageModel : AuthorizePageModel
    {
        private readonly LessonInteractor lessonInteractor;

        protected LessonInteractor LessonInteractor => lessonInteractor;

        public LessonsBasePageModel(LessonInteractor lessonInteractor)
        {
            this.lessonInteractor = lessonInteractor;
        }
    }
}
