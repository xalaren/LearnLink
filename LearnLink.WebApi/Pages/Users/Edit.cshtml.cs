using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Users
{
    public class EditModel : UsersPageModel
    {
        public EditModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response? QueryResult { get; set; }

        public UserDto? UserDto { get; set; }


        public async Task<IActionResult> OnGet(int userId)
        {
            RequireAuthorize();

            if(!AdminAuthorized)
            {
                return AccessDeniedPage();
            }

            if(userId == 0)
            {
                return Page();
            }

            var response = await UserInteractor.GetUserAsync(userId);

            if(response.Success)
            {
                UserDto = response.Value;
                return Page();
            }

            QueryResult = response;

            return Page();
        }

        public async Task OnPost(UserDto userDto)
        {
            QueryResult = await UserInteractor.UpdateUserAsync(userDto);
        }
    }
}
