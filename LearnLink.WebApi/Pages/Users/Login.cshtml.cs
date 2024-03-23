using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.Users.PageModels;

namespace LearnLink.WebApi.Pages.Users
{
    public class LoginModel : UsersPageModel
    {
        public LoginModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response<string>? QueryResult { get; set; } 

        public async Task OnPost(string nickname, string password)
        {
            QueryResult = await UserInteractor.AuthenticateAsync(nickname, password);
        }
    }
}
