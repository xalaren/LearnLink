using CoursesPrototype.Application.Helpers;
using CoursesPrototype.Application.Mappers;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Application.Repository.BasicRepositories;
using CoursesPrototype.Application.Security;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Shared.Exceptions;
using CoursesPrototype.Shared.ToClientData.DataTransferObjects;
using CoursesPrototype.Shared.ToClientData.Responses;

namespace CoursesPrototype.Application.Interactors
{
    public class UserInteractor
    {
        private readonly IUserRepository userRepository;
        private readonly ICredentialsRepository credentialsRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEncryptionService encryptionService;
        private readonly IAuthenticationService authenticationService;

        public UserInteractor(
            IUserRepository userRepository,
            ICredentialsRepository credentialsRepository,
            IUnitOfWork unitOfWork,
            IEncryptionService encryptionService,
            IAuthenticationService authenticationService)
        {
            this.userRepository = userRepository;
            this.credentialsRepository = credentialsRepository;
            this.unitOfWork = unitOfWork;
            this.encryptionService = encryptionService;
            this.authenticationService = authenticationService;
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

                string salt = encryptionService.GetRandomString(6);
                string hashedPassword = encryptionService.GetHash(password, salt);

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
            catch(ForClientSideBaseException exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
            catch(Exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = "Регистрация не удалась. Внутренняя ошибка",
                };
            }
        }

        public async Task<Response<UserDto[]>> GetUsersAsync()
        {
            try
            {
                return new()
                {
                    Success = true,
                    Value = (await userRepository.GetAll()).Select(user => user.ToDto()).ToArray(),
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

        public async Task<Response<UserDto>> GetUserAsync(int userId)
        {
            try
            {
                var user = await userRepository.Get(userId);

                if (user == null)
                {
                    return new()
                    {
                        Success = false,
                        Message = "Пользователь не найден",
                    };
                }

                return new()
                {
                    Success = true,
                    Value = user.ToDto(),
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

        public async Task<Response<string>> AuthenticateAsync(string nickname, string password)
        {
            try
            {
                if (!ValidateHelper.ValidateToEmptyStrings(nickname, password))
                {
                    return new()
                    {
                        Success = false,
                        Message = "Не все поля были заполнены",
                    };
                }

                var user = await userRepository.GetByNickname(nickname);

                if(user == null)
                {
                    return new()
                    {
                        Success = false,
                        Message = "Пользователь не найден",
                    };
                }

                var userCredentials = await credentialsRepository.GetCredentialsByUserId(user.Id);

                if (userCredentials == null)
                {
                    return new()
                    {
                        Success = false,
                        Message = "Данные для входа не найдены",
                    };
                }

                var hashedPassword = encryptionService.GetHash(password, userCredentials.Salt);

                var authenticationResult = authenticationService.Authenticate(nickname, hashedPassword, userCredentials.HashedPassword);

                if(authenticationResult == null)
                {
                    return new()
                    {
                        Success = false,
                        Message = "Неверные логин или пароль",
                    };
                }

                return new()
                {
                    Success = true,
                    Message = "Аутентификация прошла успешно",
                    Value = authenticationResult,
                };
            }
            catch (ForClientSideBaseException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
            catch (Exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Аутентификация не удалась. Внутренняя ошибка",
                };
            }
        }
    }
}
