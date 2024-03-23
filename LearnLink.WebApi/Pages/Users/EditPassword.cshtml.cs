using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.Users.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Users
{
    public class EditPasswordModel : UsersPageModel
    {
        public EditPasswordModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response? QueryResult { get; set; }

        public async Task OnPost(int userId, string oldPassword, string newPassword)
        {
            QueryResult = await UserInteractor.UpdateUserPasswordAsync(userId, oldPassword, newPassword);
        }
    }
}
