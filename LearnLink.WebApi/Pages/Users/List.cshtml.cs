using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Users
{
    public class ListModel : UsersBasePageModel
    {
        public ListModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response<UserDto[]>? QueryResult { get; set; }
        public UserDto[]? Users { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return await AuthRequiredAsync(async () =>
            {
                QueryResult = await UserInteractor.GetUsersAsync();

                if (QueryResult.Success)
                {
                    Users = QueryResult.Value;
                }
            });
        }
    }
}
