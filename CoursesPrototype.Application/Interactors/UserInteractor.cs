using CoursesPrototype.Application.Helpers;
using CoursesPrototype.Application.Mappers;
using CoursesPrototype.Application.RepositoryInterfaces;
using CoursesPrototype.Application.Security;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Shared.ToClientData.DataTransferObjects;
using CoursesPrototype.Shared.ToClientData.Responses;

namespace CoursesPrototype.Application.Interactors
{
    public class UserInteractor
    {
        private readonly IAsyncRepository<User> userRepository;
        private readonly IAsyncRepository<Credentials> credentialsRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserInteractor(IAsyncRepository<User> userRepository, IAsyncRepository<Credentials> credentialsRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.credentialsRepository = credentialsRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response> RegisterAsync(UserDto userDto, string password)
        {
            if(userDto == null)
            {
                return new Response()
                {
                    Success = false,
                    Message = "Пользователь не был передан",
                };
            }

            try
            {
                if(!ValidateHelper.ValidateToEmptyStrings(password))
                {
                    return new Response()
                    {
                        Success = false,
                        Message = "Пароль не был заполнен",
                    };
                }

                var user = userDto.ToEntity();

                await userRepository.Create(user);
                unitOfWork.Commit();

                string salt = SaltGenerator.Generate(6);
                string hashedPassword = HashGenerator.Generate(password, salt);

                var userCredentials = new Credentials()
                {
                    UserId = user.Id,
                    HashedPassword = hashedPassword,
                    Salt = salt,
                };
                await credentialsRepository.Create(userCredentials);

                unitOfWork.Commit();

                return new Response()
                {
                    Success = true,
                    Message = "Пользователь успешно зарегистрирован",
                };
            }
            catch(Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
        }

        public async Task<Response<UserDto[]>> GetUsers()
        {
            try
            {
                await Task.Delay(100);

                return new()
                {
                    Success = true,
                    Value = userRepository.GetAll().Select(user => user.ToDto()).ToArray(),
                };

            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
        }
    }
}
