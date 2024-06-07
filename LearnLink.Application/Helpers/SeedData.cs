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

        public async Task InitializeAdmin(string adminNickname, string adminPassword)
        {
            try
            {
                if (unitOfWork.Users.Any()) return;

                var admin = new UserDto()
                {
                    Id = 0,
                    Nickname = adminNickname,
                    Name = AdminUserDataConstants.ADMIN_USER_NAME,
                    Lastname = AdminUserDataConstants.ADMIN_LASTNAME,
                };

                await userInteractor.RegisterAsync(admin, adminPassword, RoleDataConstants.ADMIN_ROLE_ID);

            }
            catch (Exception)
            {
                //Catch statement actions
            }
        }

        public async Task InitializeAdminRole()
        {
            try
            {
                var existRole = await roleInteractor.GetRoleBySignAsync(RoleDataConstants.ADMIN_ROLE_SIGN);

                if (existRole.Value != null) return;

                var adminRole = new RoleDto(
                    Id: 0,
                    Name: RoleDataConstants.ADMIN_ROLE_NAME,
                    Sign: RoleDataConstants.ADMIN_ROLE_SIGN,
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
                var existRole = await roleInteractor.GetRoleBySignAsync(RoleDataConstants.USER_ROLE_SIGN);

                if (existRole.Value != null) return;

                var userRole = new RoleDto(
                    Id: 0,
                    Name: RoleDataConstants.USER_ROLE_NAME,
                    Sign: RoleDataConstants.USER_ROLE_SIGN,
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
                var existRole = await roleInteractor.GetRoleBySignAsync(RoleDataConstants.MODERATOR_ROLE_SIGN);

                if (existRole.Value != null) return;

                var userRole = new LocalRoleDto()
                {
                    Id = 0,
                    Name = RoleDataConstants.MODERATOR_ROLE_NAME,
                    Sign = RoleDataConstants.MODERATOR_ROLE_SIGN,
                    ViewAccess = true,
                    EditAccess = true,
                    RemoveAccess = true,
                    ManageInternalAccess = true,
                    InviteAccess = true,
                    KickAccess = true,
                    EditRolesAccess = true,
                };

                await localRoleInteractor.CreateLocalRoleAsync(userRole, true);
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
                var existRole = await roleInteractor.GetRoleBySignAsync(RoleDataConstants.MEMBER_ROLE_SIGN);

                if (existRole.Value != null) return;

                var userRole = new LocalRoleDto()
                {
                    Id = 0,
                    Name = RoleDataConstants.MEMBER_ROLE_NAME,
                    Sign = RoleDataConstants.MEMBER_ROLE_SIGN,
                    ViewAccess = true,
                    EditAccess = false,
                    RemoveAccess = false,
                    ManageInternalAccess = false,
                    InviteAccess = false,
                    KickAccess = false,
                    EditRolesAccess = false,
                };

                await localRoleInteractor.CreateLocalRoleAsync(userRole, true);
            }
            catch (Exception)
            {
                //Catch statement actions
            }
        }
    }
}
