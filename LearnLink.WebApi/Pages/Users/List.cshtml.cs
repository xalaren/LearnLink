using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Users
{
    public class ListModel : PageModel
    {
        private readonly UserInteractor userInteractor;

        public ListModel(UserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }

        public Response<UserDto[]>? QueryResult { get; set; }
        public UserDto[]? Users { get; set; }

        public async Task OnGet()
        {
            QueryResult = await userInteractor.GetUsersAsync();
            
            if(QueryResult.Success)
            {
                Users = QueryResult.Value;
            }
        }
    }
}
