using LearnLink.Application.Helpers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Constants;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class UserVerifierService
    {
        private readonly IUnitOfWork unitOfWork;
        public UserVerifierService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response> VerifyUserAsync(string? nickname, int userId)
        {
            try
            {
                if (!ValidationHelper.ValidateToEmptyStrings(nickname))
                {
                    throw new AccessLevelException("Доступ отклонен");
                }

                var userByNickname = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);

                var adminRole = await unitOfWork.Roles.FirstOrDefaultAsync(role => role.Sign == RoleSignConstants.ADMIN);

                if(adminRole == null)
                {
                    throw new CustomException("Ошибка при получении роли пользователя");
                }

                if (userByNickname == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                unitOfWork.Users.Entry(userByNickname)
                            .Reference(u => u.Role)
                            .Load();


                if (userByNickname.Id != userId && userByNickname.Role.Id != adminRole.Id)
                {
                    throw new AccessLevelException("Доступ отклонен");
                }

                return new Response()
                {
                    Success = true,
                    Message = "Верификация прошла успешно",
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
                    Message = "Верификация не удалась",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }
    }
}
