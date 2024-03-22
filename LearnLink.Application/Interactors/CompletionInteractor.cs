using LearnLink.Application.Transaction;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class CompletionInteractor(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        //public async Task<Response> CompleteModuleAsync(int userId, int moduleId)
        //{
        //    try
        //    {
        //        var moduleCompletion = await unitOfWork.ModuleCompletions.FirstOrDefaultAsync(moduleCompletion => moduleCompletion.UserId == userId && moduleCompletion.ModuleId == moduleId);

        //        if (moduleCompletion == null)
        //        {
        //            throw new NotFoundException("Прогресс модуля не найден");
        //        }

        //        moduleCompletion.Completed = true;
        //    }
        //    catch (CustomException exception)
        //    {
        //        return new()
        //        {
        //            Success = false,
        //            Message = exception.Message,
        //        };
        //    }
        //    catch (Exception exception)
        //    {
        //        return new()
        //        {
        //            Success = false,
        //            Message = "Не удалось выполнить модуль",
        //            InnerErrorMessages = [exception.Message]
        //        };
        //    }
        //}
    }
}
