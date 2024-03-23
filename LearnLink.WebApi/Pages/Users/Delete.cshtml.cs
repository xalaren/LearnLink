using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.Users.PageModels;

namespace LearnLink.WebApi.Pages.Users
{
    public class DeleteModel : UsersPageModel
    {
        public DeleteModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response? QueryResult { get; set; }

        public async Task OnPost(int userId)
        {
            QueryResult = await UserInteractor.RemoveUserAsync(userId);
        }
    }
}
