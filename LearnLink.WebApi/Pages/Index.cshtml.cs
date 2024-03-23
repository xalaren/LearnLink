using LearnLink.Application.Interactors;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserInteractor userInteractor;
        public string? ErrorText { get; set; }
        public string? SuccessText { get; set; }

        public IndexModel(ILogger<IndexModel> logger, UserInteractor userInteractor)
        {
            _logger = logger;
            this.userInteractor = userInteractor;
        }

        public async Task OnPost(string login, string password)
        {
            var result = await userInteractor.AuthenticateAsync(login, password, true);

            if (!result.Success)
            {
                ErrorText = result.Message;
                return;
            }

            SuccessText = result.Message;
        }
    }
}
