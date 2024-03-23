using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.Users.PageModels;

namespace LearnLink.WebApi.Pages.Users
{
    public class FindModel : UsersPageModel
    {
        public FindModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response<UserDto>? QueryResult { get; set; }
        public UserDto? FoundUser { get; set; }

        public async Task OnGet(string nickname)
        {
            if(nickname == null) return;
            QueryResult = await UserInteractor.GetUserByNicknameAsync(nickname);

            if(QueryResult.Success)
            {
                FoundUser = QueryResult.Value;
            }
        }
    }
}
