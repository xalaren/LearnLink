using LearnLink.Application.Interactors;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class CourseLocalRolesBasePageModel : AuthorizePageModel
    {
        private readonly CourseLocalRoleInteractor courseLocalRoleInteractor;

        protected CourseLocalRoleInteractor CourseLocalRoleInteractor => courseLocalRoleInteractor;

        public CourseLocalRolesBasePageModel(CourseLocalRoleInteractor courseLocalRoleInteractor)
        {
            this.courseLocalRoleInteractor = courseLocalRoleInteractor;
        }
    }
}
