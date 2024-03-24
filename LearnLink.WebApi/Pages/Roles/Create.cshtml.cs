using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Roles
{
    public class CreateModel : RolesBasePageModel
    {
        public CreateModel(RoleInteractor roleInteractor) : base(roleInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(string name, string sign, string? isAdmin)
        {
            bool admin = string.IsNullOrWhiteSpace(isAdmin) ? false : true;
            var roleDto = new RoleDto(0, name, sign, admin);

            QueryResult = await RoleInteractor.CreateRoleAsync(roleDto);
        }
    }
}
