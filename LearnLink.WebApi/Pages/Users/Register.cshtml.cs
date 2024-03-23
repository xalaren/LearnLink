using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.Users.PageModels;

namespace LearnLink.WebApi.Pages.Users
{
    public class RegisterModel : UsersPageModel
    {
        public RegisterModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response? QueryResult { get; set; } = null;

        public async Task OnPost(string nickname, string password, string name, string lastname, int? roleId, IFormFile avatar)
        {
            UserDto userDto = new()
            {
                Nickname = nickname,
                Name = name,
                Lastname = lastname,
                AvatarFormFile = avatar
            };

            QueryResult = await UserInteractor.RegisterAsync(userDto, password, roleId ?? 0);
        }
    }
}
