using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
namespace LearnLink.WebApi.Pages.PageModels
{
    public class CoursesBasePageModel : AuthorizePageModel
    {
        private readonly CourseInteractor courseInteractor;

        protected CourseInteractor CourseInteractor => courseInteractor;

        public CoursesBasePageModel(CourseInteractor courseInteractor)
        {
            this.courseInteractor = courseInteractor;
        }
    }
}
