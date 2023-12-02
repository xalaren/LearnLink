using CoursesPrototype.Application.Helpers;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Exceptions;
using CoursesPrototype.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Application.Interactors
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

                var user = await unitOfWork.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Nickname == nickname);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                if (user.Id != userId)
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
