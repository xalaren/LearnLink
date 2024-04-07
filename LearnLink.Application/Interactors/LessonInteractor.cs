using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class LessonInteractor(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Response<LessonDto[]>> GetAllLessons()
        {
            try
            {
                var lessons = await unitOfWork.Lessons
                    .AsNoTracking()
                    .Select(lesson => lesson.ToDto())
                    .ToArrayAsync();

                return new()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Уроки успешно получены",
                    Value = lessons
                };
            }
            catch(CustomException exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch(Exception exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить уроки",
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response> CreateLesson(int moduleId, int courseId, LessonDto lessonDto)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(lessonDto.Title))
                {
                    throw new ValidationException("Название урока не было заполнено");
                }

                var lesson = lessonDto.ToEntity();

                var module = await unitOfWork.Modules.FirstOrDefaultAsync(module => module.Id == moduleId);

                if(module == null)
                {
                    throw new NotFoundException("Модуль не найден");
                }

                var moduleLesson = new ModuleLesson()
                {
                    Lesson = lesson,
                    Module = module,
                };

                await unitOfWork.ModuleLessons.AddAsync(moduleLesson);
                await unitOfWork.Lessons.AddAsync(lesson);

                var courseCompletions = unitOfWork.CourseCompletions.Where(courseCompletion => courseCompletion.CourseId == courseId).Include(courseCompletion => courseCompletion.User);

                if (courseCompletions.Count() > 0)
                {
                    var lessonCompletions = courseCompletions.Select(sub => new LessonCompletion()
                    {
                        User = sub.User,
                        Module = module,
                        Lesson = lesson,
                        Completed = false,
                        CompletionProgress = 0,
                    });

                    await unitOfWork.LessonCompletions.AddRangeAsync(lessonCompletions);
                }

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                };
            }
            catch (CustomException exception)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось создать урок",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }
    }

}
