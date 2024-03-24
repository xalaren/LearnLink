using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Users
{
    public class FindModel : UsersBasePageModel
    {
        public FindModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response<UserDto>? QueryResult { get; set; }
        public UserDto? FoundUser { get; set; }

        public async Task<IActionResult> OnGet(string nickname)
        {
            RequireAuthorize();

            if (!AdminAuthorized)
            {
                return AccessDeniedPage();
            }

            
            if (nickname == null) return Page();
            QueryResult = await UserInteractor.GetUserByNicknameAsync(nickname);

            if(QueryResult.Success)
            {
                FoundUser = QueryResult.Value;
            }

            return Page();
        }
    }
}
