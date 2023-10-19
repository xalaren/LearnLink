using CoursesPrototype.Application.Helpers;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Core.Exceptions;
using CoursesPrototype.Shared.Responses;

namespace CoursesPrototype.Application.Interactors
{
    public class UserVerifierService
    {
        private readonly IUserRepository userRepository;

        public UserVerifierService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Response> VerifyUserAsync(string? nickname, int userId)
        {
            try
            {
                if (!ValidateHelper.ValidateToEmptyStrings(nickname))
                {
                    throw new AccessLevelException("Доступ отклонен");
                }

                var user = await userRepository.GetByNicknameAsync(nickname!);

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
