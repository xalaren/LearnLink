using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Users
{
    public class RegisterModel : PageModel
    {
        private readonly UserInteractor userInteractor;

        public RegisterModel(UserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }

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

            QueryResult = await userInteractor.RegisterAsync(userDto, password, roleId ?? 0);
        }
    }
}
