using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Shared.DataTransferObjects;

namespace CoursesPrototype.Application.Helpers
{
    public class SeedData
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserInteractor userInteractor;

        public SeedData(IUnitOfWork unitOfWork, UserInteractor userInteractor)
        {
            this.unitOfWork = unitOfWork;
            this.userInteractor = userInteractor;
        }

        public async void InitializeAdmin()
        {
            var admin = new UserDto()
            {
                Id = 1,
                Nickname = "admin",
                Lastname = "AdminLastname",
                Name = "AdminName",
            };

            var password = "AdminPass01@";

            if (unitOfWork.Users.Any()) return;

            await userInteractor.RegisterAsync(admin, password);
        }
    }
}
