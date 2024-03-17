using LearnLink.Application.Helpers;
using LearnLink.Application.Transaction;
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

                if (userByNickname == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                await unitOfWork.Users
                    .Entry(userByNickname)
                    .Reference(u => u.Role)
                    .LoadAsync();

                var userById = await unitOfWork.Users.FindAsync(userId);

                if(userById == null)
                {
                    throw new NotFoundException("Пользватель не найден");
                }

                await unitOfWork.Users
                    .Entry(userById)
                    .Reference(u => u.Role)
                    .LoadAsync();

                if (!userByNickname.Role.IsAdmin && userByNickname.Id != userById.Id)
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }
    }
}
