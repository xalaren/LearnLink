using CoursesPrototype.Application.Helpers;
using CoursesPrototype.Application.Mappers;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Application.Security;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Core.Exceptions;
using CoursesPrototype.Shared.DataTransferObjects;
using CoursesPrototype.Shared.Responses;

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
                    throw new ValidationException("Пароль не был заполнен");
                }

                var user = userDto.ToEntity();

                await userRepository.CreateAsync(user);
                unitOfWork.Commit();

                string salt = encryptionService.GetRandomString(SALT_LENGTH);
                string hashedPassword = encryptionService.GetHash(password, salt);

                var userCredentials = new Credentials()
                {
                    UserId = user.Id,
                    HashedPassword = hashedPassword,
                    Salt = salt,
                };
                await credentialsRepository.CreateAsync(userCredentials);

                unitOfWork.Commit();

                return new Response()
                {
                    Success = true,
                    Message = "Пользователь успешно зарегистрирован",
                };
            }
            catch(CustomException exception)
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
            catch (CustomException exception)
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
                    Message = "Не удалось получить данные",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response<UserDto>> GetUserAsync(int userId)
        {
            try
            {
                var user = await userRepository.GetAsync(userId);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                return new()
                {
                    Success = true,
                    Value = user.ToDto(),
                };
            }
            catch (CustomException exception)
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
                    Message = "Не удалось получить данные",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response<string>> AuthenticateAsync(string nickname, string password)
        {
            try
            {
                if (!ValidateHelper.ValidateToEmptyStrings(nickname, password))
                {
                    throw new ValidationException("Не все поля были заполнены");
                }

                var user = await userRepository.GetByNicknameAsync(nickname);

                if(user == null)
                {
                    throw new NotFoundException("Неверное имя пользователя");
                }

                var userCredentials = await credentialsRepository.GetCredentialsByUserId(user.Id);

                if (userCredentials == null)
                {
                    throw new NotFoundException("Данные для входа не найдены");
                }

                var hashedPassword = encryptionService.GetHash(password, userCredentials.Salt);

                var authenticationResult = authenticationService.Authenticate(nickname, hashedPassword, userCredentials.HashedPassword);

                if(authenticationResult == null)
                {
                    throw new ValidationException("Неверный пароль");
                }

                return new()
                {
                    Success = true,
                    Message = "Аутентификация прошла успешно",
                    Value = authenticationResult,
                };
            }
            catch (CustomException exception)
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

        public async Task<Response> RemoveUserAsync(int userId)
        {
            try
            {
                var user = await userRepository.GetAsync(userId);

                await userRepository.RemoveAsync(userId);

                unitOfWork.Commit();

                return new()
                {
                    Success = true,
                    Message = "Учетная запись удалена успешно",
                };
            }
            catch (CustomException exception)
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
                    Message = "Удаление не удалось",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> UpdateUserAsync(UserDto userDto)
        {
            try
            {
                var user = await userRepository.GetAsync(userDto.Id);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                if (user.Id != userDto.Id)
                {
                    throw new AccessLevelException("Изменение пользователя отклонено");
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
            catch (CustomException exception)
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
                    Message = "Изменение не удалось",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> UpdateUserPasswordAsync(int userId, string oldPassword, string newPassword)
        {
            try
            {
                var user = await userRepository.GetAsync(userId);

                if (!ValidateHelper.ValidateToEmptyStrings(oldPassword, newPassword))
                {
                    throw new ValidationException("Пароли не были заполнены");
                }

                if(user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                var userCredentials = await credentialsRepository.GetCredentialsByUserId(user.Id);

                if (userCredentials == null)
                {
                    throw new NotFoundException("Данные для входа не найдены");
                }

                var hashedPassword = encryptionService.GetHash(oldPassword, userCredentials.Salt);
                var authenticationResult = authenticationService.Authenticate(user.Nickname, hashedPassword, userCredentials.HashedPassword);

                if (authenticationResult == null)
                {
                    throw new ValidationException("Неверный пароль");
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
            catch (CustomException exception)
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
                    Message = "Изменение не удалось",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }
    }
}
