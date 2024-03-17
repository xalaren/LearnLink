using System.IO;
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
        private readonly LocalRoleInteractor localRoleInteractor;

        public SeedData(IUnitOfWork unitOfWork, UserInteractor userInteractor, RoleInteractor roleInteractor, LocalRoleInteractor localRoleInteractor)
        {
            this.unitOfWork = unitOfWork;
            this.userInteractor = userInteractor;
            this.roleInteractor = roleInteractor;
            this.localRoleInteractor = localRoleInteractor;
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

                string password = "admin";

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

                var adminRole = new RoleDto(
                    Id: 1,
                    Name: "Администратор",
                    Sign: RoleSignConstants.ADMIN,
                    IsAdmin: true
                );

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

                var userRole = new RoleDto(
                    Id: 2,
                    Name: "Пользователь",
                    Sign: RoleSignConstants.USER,
                    IsAdmin: false
                );

                await roleInteractor.CreateRoleAsync(userRole);
            }
            catch (Exception)
            {
                //Catch statement actions
            }
        }

        public async Task InitializeModeratorLocalRole()
        {
            try
            {
                var existRole = await roleInteractor.GetRoleBySignAsync(RoleSignConstants.MODERATOR);

                if (existRole.Value != null) return;

                var userRole = new LocalRoleDto(
                    Id: 3,
                    Name: "Модератор",
                    Sign: RoleSignConstants.MODERATOR,
                    ViewAccess: true,
                    EditAccess: true,
                    RemoveAccess: true,
                    ManageInternalAccess: true,
                    InviteAccess: true
                );

                await localRoleInteractor.CreateLocalRoleAsync(userRole);
            }
            catch (Exception)
            {
                //Catch statement actions
            }
        }

        public async Task InitializeMemberLocalRole()
        {
            try
            {
                var existRole = await roleInteractor.GetRoleBySignAsync(RoleSignConstants.MEMBER);

                if (existRole.Value != null) return;

                var userRole = new LocalRoleDto(
                    Id: 4,
                    Name: "Участник",
                    Sign: RoleSignConstants.MEMBER,
                    ViewAccess: true,
                    EditAccess: false,
                    RemoveAccess: false,
                    ManageInternalAccess: false,
                    InviteAccess: false
                );

                await localRoleInteractor.CreateLocalRoleAsync(userRole);
            }
            catch (Exception)
            {
                //Catch statement actions
            }
        }
    }
}
