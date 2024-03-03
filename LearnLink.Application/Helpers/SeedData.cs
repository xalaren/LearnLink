using LearnLink.Application.Interactors;
using LearnLink.Application.Transaction;
using LearnLink.Core.Constants;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Helpers
{
    public class SeedData
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserInteractor userInteractor;
        private readonly RoleInteractor roleInteractor;

        public SeedData(IUnitOfWork unitOfWork, UserInteractor userInteractor, RoleInteractor roleInteractor)
        {
            this.unitOfWork = unitOfWork;
            this.userInteractor = userInteractor;
            this.roleInteractor = roleInteractor;
        }

        public async Task InitializeAdmin()
        {
            try
            {
                if (unitOfWork.Users.Any()) return;

                var admin = new UserDto()
                {
                    Id = 1,
                    Nickname = "admin",
                    Name = "AdminName",
                    Lastname = "AdminLastname",
                };

                string password = "AdminSystemPass0!";

                await userInteractor.RegisterAsync(admin, password, 1);
                
            }
            catch(Exception)
            {
                //Catch statement actions
            }
        }

        public async Task InitializeAdminRole()
        {
            try
            {
                var existRole = await roleInteractor.GetRoleBySignAsync(RoleSignConstants.ADMIN);

                if (existRole.Value != null) return;

                var adminRole = new RoleDto()
                {
                    Id = 1,
                    Name = "Администратор",
                    Sign = RoleSignConstants.ADMIN,
                };

                await roleInteractor.CreateRoleAsync(adminRole);
            }
            catch (Exception)
            {
                //Catch statement actions
            }
        }

        public async Task InitializeUserRole()
        {
            try
            {
                var existRole = await roleInteractor.GetRoleBySignAsync(RoleSignConstants.USER);

                if (existRole.Value != null) return;

                var userRole = new RoleDto()
                {
                    Id = 2,
                    Name = "Пользователь",
                    Sign = RoleSignConstants.USER,
                };

                await roleInteractor.CreateRoleAsync(userRole);
            }
            catch (Exception)
            {
                //Catch statement actions
            }
        }
    }
}
