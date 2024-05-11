using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Subscriptions
{
    public class InviteModel : SubscriptionsBasePageModel
    {
        private readonly UserInteractor userInteractor;

        public InviteModel(SubscriptionInteractor subscriptionInteractor, UserInteractor userInteractor) : base(subscriptionInteractor)
        {
            this.userInteractor = userInteractor;
        }

        public Response? QueryResult { get; set; }

        public DataPage? DataPage { get; set; }

        public UserDto[]? Users { get; set; }

        public async Task<IActionResult> OnGet(int courseId, string? searchText, int pageNumber, int pageSize)
        {
            return await AuthRequiredAsync(async () =>
            {
                if (pageNumber == 0 || pageSize == 0) return;

                var result = await userInteractor.FindUsersExceptCourseUsersAsync(courseId, searchText, new DataPageHeader(pageNumber, pageSize));

                if(result.Success && result.Value != null && result.Value.Values != null)
                {
                    Users = result.Value.Values;
                    DataPage = result.Value;
                }

                QueryResult = result;
            });
        }

        public async Task OnPost(int userId, int courseId, int localRoleId, string[] selectedUsersStrings)
        {
            int[] idenitifiers = selectedUsersStrings.Select(int.Parse).ToArray();

            QueryResult = await SubscriptionInteractor.InviteAsync(userId, courseId, localRoleId, idenitifiers);
        }
    }
}
