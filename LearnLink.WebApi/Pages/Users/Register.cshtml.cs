using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Users
{
    public class RegisterModel : UsersBasePageModel
    {
        public RegisterModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response? QueryResult { get; set; } = null;

        public IActionResult OnGet()
        {
            RequireAuthorize();

            if (!AdminAuthorized)
            {
                return AccessDeniedPage();
            }

            return Page();
        }

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
