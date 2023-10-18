using CoursesPrototype.Application.Helpers;
using CoursesPrototype.Application.Mappers;
using CoursesPrototype.Application.Repository;
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

        private const int SALT_LENGTH = 6;

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
                throw new ArgumentNullException("", "UserDto was null");
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

                string salt = encryptionService.GetRandomString(SALT_LENGTH);
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
            catch(Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = "Регистрация не удалась. Внутренняя ошибка",
                    InnerErrorMessages = new string[] { exception.Message },
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
                        Message = "Неверное имя пользователя",
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
                        Message = "Неверный пароль",
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
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Аутентификация не удалась. Внутренняя ошибка",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> RemoveUserAsync(string nickname, int userId)
        {
            try
            {
                if (!ValidateHelper.ValidateToEmptyStrings(nickname))
                {
                    return new()
                    {
                        Success = false,
                        Message = "Пользователь не найден",
                    };
                }

                var user = await userRepository.GetByNickname(nickname);

                if (user == null)
                {
                    return new()
                    {
                        Success = false,
                        Message = "Пользователь не найден",
                    };
                }

                if(user.Id != userId)
                {
                    return new()
                    {
                        Success = false,
                        Message = "Удаление отклонено",
                    };
                }

                await userRepository.Remove(userId);

                unitOfWork.Commit();

                return new()
                {
                    Success = true,
                    Message = "Учетная запись удалена успешно",
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
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Удаление не удалось. Внутренняя ошибка",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> UpdateUserAsync(string nickname, UserDto userDto)
        {
            try
            {
                if (!ValidateHelper.ValidateToEmptyStrings(nickname))
                {
                    return new()
                    {
                        Success = false,
                        Message = "Пользователь не найден",
                    };
                }

                var user = await userRepository.GetByNickname(nickname);

                if (user == null)
                {
                    return new()
                    {
                        Success = false,
                        Message = "Пользователь не найден",
                    };
                }

                if (user.Id != userDto.Id)
                {
                    return new()
                    {
                        Success = false,
                        Message = "Изменение пользователя отклонено",
                    };
                }

                var updatedEntity = user.Assign(userDto);

                userRepository.Update(user.Assign(userDto));

                unitOfWork.Commit();

                return new()
                {
                    Success = true,
                    Message = "Пользователь успешно изменен",
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
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Изменение не удалось. Внутренняя ошибка",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> UpdateUserPasswordAsync(string nickname, int userId, string oldPassword, string newPassword)
        {
            try
            {
                if (!ValidateHelper.ValidateToEmptyStrings(nickname))
                {
                    return new()
                    {
                        Success = false,
                        Message = "Пользователь не найден",
                    };
                }

                var user = await userRepository.GetByNickname(nickname);

                if (user == null)
                {
                    return new()
                    {
                        Success = false,
                        Message = "Пользователь не найден",
                    };
                }

                if (user.Id != userId)
                {
                    return new()
                    {
                        Success = false,
                        Message = "Изменение пароля пользователя отклонено",
                    };
                }

                if (!ValidateHelper.ValidateToEmptyStrings(oldPassword, newPassword))
                {
                    return new()
                    {
                        Success = false,
                        Message = "Пароли не были заполнены",
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

                var hashedPassword = encryptionService.GetHash(oldPassword, userCredentials.Salt);
                var authenticationResult = authenticationService.Authenticate(nickname, hashedPassword, userCredentials.HashedPassword);

                if (authenticationResult == null)
                {
                    return new()
                    {
                        Success = false,
                        Message = "Неверный пароль",
                    };
                }

                var newSalt = encryptionService.GetRandomString(SALT_LENGTH);
                var newHash = encryptionService.GetHash(newPassword, newSalt);

                userCredentials.Salt = newSalt;
                userCredentials.HashedPassword = newHash;

                credentialsRepository.Update(userCredentials);

                unitOfWork.Commit();

                return new()
                {
                    Success = true,
                    Message = "Пароль успешно изменен",
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
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Изменение не удалось. Внутренняя ошибка",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }
    }
}
