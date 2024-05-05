using LearnLink.Application.Helpers;
using LearnLink.Application.Mappers;
using LearnLink.Application.Security;
using LearnLink.Application.Transaction;
using LearnLink.Core.Constants;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class UserInteractor
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEncryptionService encryptionService;
        private readonly IAuthenticationService authenticationService;
        private readonly DirectoryStore directoryStore;

        public UserInteractor(
            IUnitOfWork unitOfWork,
            IEncryptionService encryptionService,
            IAuthenticationService authenticationService,
            DirectoryStore directoryStore)
        {
            this.unitOfWork = unitOfWork;
            this.encryptionService = encryptionService;
            this.authenticationService = authenticationService;
            this.directoryStore = directoryStore;
        }

        public async Task<Response<string>> AuthenticateAsync(string nickname, string password, bool authenticateAdmin = false)
        {
            try
            {
                if (!ValidationHelper.ValidateToEmptyStrings(nickname, password))
                {
                    throw new ValidationException("Не все поля были заполнены");
                }

                var user = await unitOfWork.Users.FirstOrDefaultAsync(user => user.Nickname == nickname);

                if (user == null)
                {
                    throw new NotFoundException("Неверное имя пользователя");
                }

                await unitOfWork.Users.Entry(user).Reference(user => user.Role).LoadAsync();

                if (authenticateAdmin && !user.Role.IsAdmin)
                {
                    throw new AccessLevelException("Аутентификация администратора не пройдена");
                }

                var userCredentials = await unitOfWork.Credentials.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == user.Id);

                if (userCredentials == null)
                {
                    throw new NotFoundException("Данные для входа не найдены");
                }

                var hashedPassword = encryptionService.GetHash(password, userCredentials.Salt);

                var authenticationResult = authenticationService.Authenticate(nickname, hashedPassword, userCredentials.HashedPassword, user.Role.Sign);

                if (authenticationResult == null)
                {
                    throw new ValidationException("Неверный пароль");
                }

                return new()
                {
                    Success = true,
                    Message = "Аутентификация прошла успешно",
                    Value = authenticationResult,
                    StatusCode = 200,
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                    StatusCode = exception.StatusCode,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Аутентификация не удалась. Внутренняя ошибка",
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response> RegisterAsync(UserDto userDto, string password, int roleId = 0)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException("", "UserDto was null");
            }

            try
            {
                if (!ValidationHelper.ValidateToEmptyStrings(password))
                {
                    throw new ValidationException("Пароль не был заполнен");
                }

                var existingUser = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Nickname == userDto.Nickname);

                if (existingUser != null)
                {
                    throw new ValidationException("Данный пользователь уже зарегистрирован в системе");
                }

                var user = userDto.ToEntity();

                var role = await unitOfWork.Roles.FindAsync(roleId);

                if (role == null)
                {
                    role = await unitOfWork.Roles.FirstOrDefaultAsync(x => x.Sign == RoleSignConstants.USER);
                }


                user.Role = role!;

                if (userDto.AvatarFormFile != null)
                {
                    user.AvatarFileName = GenerateAvatarFileName(user.Nickname, userDto.AvatarFormFile.FileName);
                }

                await unitOfWork.Users.AddAsync(user);
                await unitOfWork.CommitAsync();

                if (userDto.AvatarFormFile != null)
                {
                    using (var avatarStream = userDto.AvatarFormFile.OpenReadStream())
                    {
                        await SaveAvatarAsync(avatarStream, user);
                    }
                }


                string salt = encryptionService.GetRandomString(EncryptionConstants.SALT_LENGTH);
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
            catch (CustomException exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = exception.Message,
                    StatusCode = exception.StatusCode,
                };
            }
            catch (Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = "Не удалось зарегистрироваться",
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response<UserDto[]>> GetUsersAsync()
        {
            try
            {
                var users = await unitOfWork.Users.Include(user => user.Role).Select(x => x.ToDto()).ToArrayAsync();

                return new()
                {
                    Success = true,
                    Value = users,
                    StatusCode = 200,
                    Message = "Пользователи успешно получены"
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                    StatusCode = exception.StatusCode,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Не удалось получить данные",
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response<UserDto>> GetUserAsync(int userId)
        {
            try
            {
                var user = await unitOfWork.Users.FindAsync(userId);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                await unitOfWork.Users.Entry(user)
                    .Reference(user => user.Role)
                    .LoadAsync();

                var userDto = user.ToDto();

                return new()
                {
                    Success = true,
                    Value = userDto,
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response<UserDto>> GetUserByNicknameAsync(string? nickname)
        {
            try
            {
                if (!ValidationHelper.ValidateToEmptyStrings(nickname))
                {
                    throw new ValidationException("Имя пользователя не указано, либо пользователь не найден");
                }

                var user = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                unitOfWork.Users.Entry(user).Reference(x => x.Role).Load();

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

        public async Task<Response<DataPage<UserDto[]>>> FindUsersAsync(string? searchString, DataPageHeader pageHeader)
        {
            try
            {
                var usersQuery = unitOfWork.Users
                    .AsNoTracking();


                if (searchString != null)
                {
                    usersQuery = usersQuery.Where(user =>
                            user.Id != 1 &&
                            (user.Nickname.Contains(searchString) ||
                            user.Lastname.Contains(searchString) ||
                            user.Name.Contains(searchString))
                    );
                }

                int total = usersQuery.Count();

                var users = await usersQuery
                    .OrderBy(user => user.Lastname)
                    .ThenBy(user => user.Name)
                    .ThenBy(user => user.Nickname)
                    .Skip((pageHeader.PageNumber - 1) * pageHeader.PageSize)
                    .Take(pageHeader.PageSize)
                    .Select(user => user.ToDto())
                    .ToArrayAsync();

                var dataPage = new DataPage<UserDto[]>()
                {
                    ItemsCount = total,
                    PageSize = pageHeader.PageSize,
                    PageNumber = pageHeader.PageNumber,
                    Values = users
                };

                return new()
                {
                    Success = true,
                    Message = "Пользователи найдены успешно",
                    StatusCode = 200,
                    Value = dataPage,
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                    StatusCode = exception.StatusCode,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Не удалось найти пользователей",
                    InnerErrorMessages = [exception.Message],
                    StatusCode = 500
                };
            }
        }

        public async Task<Response> RemoveUserAsync(int userId)
        {
            try
            {
                var user = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Id == userId);


                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                await unitOfWork.Users.Entry(user).Reference(x => x.Role).LoadAsync();

                if (user.Role.Sign == RoleSignConstants.ADMIN && user.Role.Id == 1)
                {
                    throw new AccessLevelException("Невозможно удалить, пользователь является системным");
                }

                RemoveAvatar(user.Id, user.AvatarFileName);
                unitOfWork.Users.Remove(user);
                await unitOfWork.CommitAsync();


                return new()
                {
                    Success = true,
                    Message = "Учетная запись удалена успешно",
                    StatusCode = 200
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                    StatusCode = exception.StatusCode,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Удаление не удалось",
                    InnerErrorMessages = [exception.Message],
                    StatusCode = 500
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

                unitOfWork.Users.Entry(user).Reference(x => x.Role).Load();

                if (!string.IsNullOrWhiteSpace(user.AvatarFileName) && userDto.AvatarFormFile != null)
                {
                    RemoveAvatar(user.Id, user.AvatarFileName);
                }

                var updatedEntity = user.Assign(userDto);

                if (userDto.AvatarFormFile != null)
                {
                    updatedEntity.AvatarFileName = GenerateAvatarFileName(updatedEntity.Nickname, userDto.AvatarFormFile.FileName);
                    using (var avatarStream = userDto.AvatarFormFile.OpenReadStream())
                    {
                        await SaveAvatarAsync(avatarStream, updatedEntity);
                    }
                }

                unitOfWork.Users.Update(updatedEntity);
                await unitOfWork.CommitAsync();

                var token = authenticationService.GetToken(updatedEntity.Nickname, updatedEntity.Role.Sign);

                return new()
                {
                    Success = true,
                    Message = "Пользователь успешно изменен",
                    Value = token,
                    StatusCode = 200
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                    StatusCode = exception.StatusCode,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Изменение не удалось",
                    InnerErrorMessages = [exception.Message],
                    StatusCode = 500
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

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                unitOfWork.Users.Entry(user).Reference(x => x.Role).Load();

                var userCredentials = await unitOfWork.Credentials.FirstOrDefaultAsync(u => u.UserId == user.Id);


                if (userCredentials == null)
                {
                    throw new NotFoundException("Данные для входа не найдены");
                }

                var hashedPassword = encryptionService.GetHash(oldPassword, userCredentials.Salt);
                var authenticationResult = authenticationService.Authenticate(user.Nickname, hashedPassword, userCredentials.HashedPassword, user.Role.Sign);

                if (authenticationResult == null)
                {
                    throw new ValidationException("Неверный пароль");
                }

                var newSalt = encryptionService.GetRandomString(EncryptionConstants.SALT_LENGTH);
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        private async Task SaveAvatarAsync(Stream? avatarStream, User user)
        {
            if (avatarStream == null || string.IsNullOrWhiteSpace(user.AvatarFileName))
            {
                return;
            }

            var directory = directoryStore.GetDirectoryPathToUserImages(user.Id);
            var avatarPath = Path.Combine(directory, user.AvatarFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(avatarPath)!);

            try
            {
                using (var fileStream = new FileStream(avatarPath, FileMode.Create))
                {
                    await avatarStream.CopyToAsync(fileStream);
                }
            }
            catch (Exception)
            {
                throw new CustomException("Не удалось сохранить аватар");
            }
        }

        private async Task<byte[]?> GetAvatarAsync(int userId, string? fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return null;
                }

                var avatarPath = Path.Combine(directoryStore.UsersStorageDirectory, userId.ToString(), fileName);

                if (!File.Exists(avatarPath))
                {
                    return null;
                }

                return await File.ReadAllBytesAsync(avatarPath);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void RemoveAvatar(int userId, string? fileName)
        {
            try
            {
                if (fileName == null)
                {
                    return;
                }

                var directory = directoryStore.GetDirectoryPathToUserImages(userId);
                var avatarPath = Path.Combine(directory, fileName);

                if (!Directory.Exists(directory) || !File.Exists(avatarPath))
                {
                    return;
                }

                File.Delete(avatarPath);
                Directory.Delete(directory);

            }
            catch (Exception)
            {
                throw new CustomException("Не удалось удалить изображение пользователя");
            }
        }

        private string GenerateAvatarFileName(string username, string fileName)
        {
            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            int year = now.Year;
            int hour = now.Hour;
            int minute = now.Minute;
            return $"{day}-{month}-{year}-{hour}h-{minute}m-{username}{Path.GetExtension(fileName)}";
        }
    }
}
