using LearnLink.Application.Interactors;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class UserCourseLocalRolesBasePageModel : AuthorizePageModel
    {
        private readonly UserCourseLocalRolesInteractor userCourseLocalRolesInteractor;

        protected UserCourseLocalRolesInteractor UserCourseLocalRolesInteractor => userCourseLocalRolesInteractor;

        public UserCourseLocalRolesBasePageModel(UserCourseLocalRolesInteractor userCourseLocalRolesInteractor)
        {
            this.userCourseLocalRolesInteractor = userCourseLocalRolesInteractor;
        }
    }
}
