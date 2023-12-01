using CoursesPrototype.Application.Helpers;
using CoursesPrototype.Application.Mappers;
using CoursesPrototype.Application.Security;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Constants;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Core.Exceptions;
using CoursesPrototype.Shared.DataTransferObjects;
using CoursesPrototype.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Application.Interactors
{
    public class UserInteractor
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEncryptionService encryptionService;
        private readonly IAuthenticationService authenticationService;

        private const int SALT_LENGTH = 6;

        public UserInteractor(
            IUnitOfWork unitOfWork,
            IEncryptionService encryptionService,
            IAuthenticationService authenticationService)
        {
            this.unitOfWork = unitOfWork;
            this.encryptionService = encryptionService;
            this.authenticationService = authenticationService;
        }

        public async Task<Response<string>> AuthenticateAsync(string nickname, string password)
        {
            try
            {
                if (!ValidationHelper.ValidateToEmptyStrings(nickname, password))
                {
                    throw new ValidationException("Не все поля были заполнены");
                }
                
                var user = await unitOfWork.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Nickname == nickname);

                if (user == null)
                {
                    throw new NotFoundException("Неверное имя пользователя");
                }

                var userCredentials = await unitOfWork.Credentials.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == user.Id);

                if (userCredentials == null)
                {
                    throw new NotFoundException("Данные для входа не найдены");
                }

                var hashedPassword = encryptionService.GetHash(password, userCredentials.Salt);

                var authenticationResult = authenticationService.Authenticate(nickname, hashedPassword, userCredentials.HashedPassword);

                if (authenticationResult == null)
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

        public async Task<Response> RegisterAsync(UserDto userDto, string password, int roleId = 0)
        {
            if(userDto == null)
            {
                throw new ArgumentNullException("", "UserDto was null");
            }

            try
            {
                if(!ValidationHelper.ValidateToEmptyStrings(password))
                {
                    throw new ValidationException("Пароль не был заполнен");
                }

                var existingUser = await unitOfWork.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Nickname == userDto.Nickname);

                if(existingUser != null)
                {
                    throw new ValidationException("Данный пользователь уже зарегистрирован в системе");
                }

                var user = userDto.ToEntity();

                var role = await unitOfWork.Roles.FindAsync(roleId);

                if (role == null)
                {
                    role = await unitOfWork.Roles.FindAsync(RoleSignConstants.USER);
                }

                user.Role = role!;

                await unitOfWork.Users.AddAsync(user);
                await unitOfWork.CommitAsync();

                string salt = encryptionService.GetRandomString(SALT_LENGTH);
                string hashedPassword = encryptionService.GetHash(password, salt);

                

                var userCredentials = new Credentials()
                {
                    UserId = user.Id,
                    HashedPassword = hashedPassword,
                    Salt = salt,
                };

                await unitOfWork.Credentials.AddAsync(userCredentials);
                await unitOfWork.CommitAsync();

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
                    Message = "Не удалось зарегистрироваться",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response<UserDto[]>> GetUsersAsync()
        {
            try
            {
                var users = await unitOfWork.Users.AsNoTracking().Select(user => user.ToDto()).ToArrayAsync();
                return new()
                {
                    Success = true,
                    Value = users,
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
                var user = await unitOfWork.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

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

        public async Task<Response<UserDto>> GetUserByNicknameAsync(string? nickname)
        {
            try
            {
                if(!ValidationHelper.ValidateToEmptyStrings(nickname))
                {
                    throw new ValidationException("Имя пользователя не указано, либо пользователь не найден");
                }

                var user = await unitOfWork.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Nickname ==  nickname);

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

        public async Task<Response> RemoveUserAsync(int userId)
        {
            try
            {
                var user = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if(user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                unitOfWork.Users.Remove(user);

                await unitOfWork.CommitAsync();

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

        public async Task<Response<string?>> UpdateUserAsync(UserDto userDto)
        {
            try
            {
                var user = await unitOfWork.Users.FindAsync(userDto.Id);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                if (user.Id != userDto.Id)
                {
                    throw new AccessLevelException("Изменение пользователя отклонено");
                }

                var updatedEntity = user.Assign(userDto);

                unitOfWork.Users.Update(updatedEntity);
                await unitOfWork.CommitAsync();

                var token = authenticationService.GetToken(updatedEntity.Nickname);

                return new()
                {
                    Success = true,
                    Message = "Пользователь успешно изменен",
                    Value = token,
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
                var user = await unitOfWork.Users.FindAsync(userId);

                if (!ValidationHelper.ValidateToEmptyStrings(oldPassword, newPassword))
                {
                    throw new ValidationException("Пароли не были заполнены");
                }

                if(user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                var userCredentials = await unitOfWork.Credentials.FirstOrDefaultAsync(u => u.UserId == user.Id);

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

                unitOfWork.Credentials.Update(userCredentials);
                await unitOfWork.CommitAsync();

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
