using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class CompletionInteractor(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Response<ModuleCompletionDto[]>> GetModuleCompletionsAsync(int userId, int courseId)
        {
            try
            {
                var moduleCompletions = unitOfWork.ModuleCompletions
                    .Where(moduleCompletion => moduleCompletion.UserId == userId && moduleCompletion.CourseId == courseId)
                    .Include(moduleCompletions => moduleCompletions.Module);

                ModuleCompletionDto[] projectedData = await moduleCompletions.Select(moduleCompletion => moduleCompletion.ToDto()).ToArrayAsync();

                return new()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Модули с прогрессами успешно получены",
                    Value = projectedData
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = 200,
                    Message = "Не удалось получить модули",
                    InnerErrorMessages = [exception.Message]
                };
            }
        }
    }
}
