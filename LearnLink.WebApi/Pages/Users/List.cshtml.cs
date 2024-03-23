using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.Users.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Users
{
    public class ListModel : UsersPageModel
    {
        public ListModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response<UserDto[]>? QueryResult { get; set; }
        public UserDto[]? Users { get; set; }

        public async Task OnGet()
        {
            QueryResult = await UserInteractor.GetUsersAsync();
            
            if(QueryResult.Success)
            {
                Users = QueryResult.Value;
            }
        }
    }
}
