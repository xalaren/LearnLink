using System.Security.Cryptography.Xml;
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

        public async Task<Response<CourseCompletionDto>> GetCourseCompletion(int userId, int courseId)
        {
            try
            {
                CourseCompletion? foundCourseCompletion =
                    await unitOfWork.CourseCompletions.FirstOrDefaultAsync(courseCompletion =>
                        courseCompletion.UserId == userId && courseCompletion.CourseId == courseId);


                if (foundCourseCompletion == null)
                {
                    throw new NotFoundException("Прогресс курса не найден");
                }

                await unitOfWork.CourseCompletions.Entry(foundCourseCompletion)
                    .Reference(courseCompletion => courseCompletion.Course)
                    .LoadAsync();

                CourseCompletionDto courseCompletionDto = foundCourseCompletion.ToDto();

                return new Response<CourseCompletionDto>()
                {
                    Success = true,
                    StatusCode = 200,
                    Value = courseCompletionDto
                };
            }
            catch (CustomException exception)
            {
                return new Response<CourseCompletionDto>()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response<CourseCompletionDto>()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить прогресс курса",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response<ModuleCompletionDto[]>> GetModuleCompletionsOfCourseAsync(int userId, int courseId)
        {
            try
            {
                var foundModuleCompletions = await unitOfWork.ModuleCompletions
                    .Where(moduleCompletion => moduleCompletion.UserId == userId && moduleCompletion.CourseId == courseId)
                    .ToListAsync();

                foreach (var moduleCompletion in foundModuleCompletions)
                {
                    await unitOfWork.ModuleCompletions.Entry(moduleCompletion)
                        .Reference(mc => mc.Course)
                        .LoadAsync();

                    await unitOfWork.ModuleCompletions.Entry(moduleCompletion)
                        .Reference(mc => mc.Module)
                        .LoadAsync();
                }

                ModuleCompletionDto[] moduleCompletions = foundModuleCompletions
                    .Select(moduleCompletion => moduleCompletion.ToDto())
                    .ToArray();

                return new()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Модули с прогрессами успешно получены",
                    Value = moduleCompletions
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

        public async Task<Response<LessonCompletionDto[]>> GetLessonCompletionsOfModuleAsync(int userId, int moduleId)
        {
            try
            {
                var foundLessonCompletions = await unitOfWork.LessonCompletions
                    .Where(lessonCompletion => lessonCompletion.UserId == userId && lessonCompletion.ModuleId == moduleId)
                    .ToListAsync();


                foreach (var lessonCompletion in foundLessonCompletions)
                {
                    await unitOfWork.LessonCompletions.Entry(lessonCompletion)
                        .Reference(lc => lc.Module)
                        .LoadAsync();

                    await unitOfWork.LessonCompletions.Entry(lessonCompletion)
                        .Reference(lc => lc.Lesson)
                        .LoadAsync();
                }

                LessonCompletionDto[] lessonCompletions = foundLessonCompletions
                    .Select(lessonCompletion => lessonCompletion.ToDto())
                    .ToArray();

                return new Response<LessonCompletionDto[]>()
                {
                    Success = true,
                    StatusCode = 200,
                    Value = lessonCompletions,
                };
            }
            catch (CustomException exception)
            {
                return new Response<LessonCompletionDto[]>()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response<LessonCompletionDto[]>()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить прогрессы уроков",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task CreateCourseCompletion(int userId, int courseId)
        {
            User? user = await unitOfWork.Users.FindAsync(userId) ?? throw new NotFoundException("Пользователь не найден");
            Course? course = await unitOfWork.Courses.FindAsync(courseId) ?? throw new NotFoundException("Курс не найден");

            CourseCompletion? courseCompletion = await unitOfWork.CourseCompletions
                .AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.UserId == userId && cp.CourseId == courseId);

            if (courseCompletion != null) return;

            courseCompletion = new CourseCompletion()
            {
                User = user,
                Course = course,
                Completed = false,
                CompletionProgress = 0,
            };

            await unitOfWork.CourseCompletions.AddAsync(courseCompletion);
        }

        public async Task CreateModuleCompletion(int userId, int moduleId)
        {
            User? user = await unitOfWork.Users.FindAsync(userId) ?? throw new NotFoundException("Пользователь не найден");
            Module? module = await unitOfWork.Modules.FindAsync(moduleId) ?? throw new NotFoundException("Модуль не найден");

            ModuleCompletion? moduleCompletion = await unitOfWork.ModuleCompletions
                .AsNoTracking()
                .FirstOrDefaultAsync(moduleCompletion => moduleCompletion.UserId == userId && moduleCompletion.ModuleId == moduleId);

            if (moduleCompletion != null) return;

            moduleCompletion = new ModuleCompletion()
            {
                User = user,
                Module = module,
                Completed = false,
                CompletionProgress = 0,
            };

            await unitOfWork.ModuleCompletions.AddAsync(moduleCompletion);
        }

        public async Task CreateLessonCompletion(int userId, int lessonId)
        {
            User? user = await unitOfWork.Users.FindAsync(userId) ?? throw new NotFoundException("Пользователь не найден");
            Lesson? lesson = await unitOfWork.Lessons.FindAsync(lessonId) ?? throw new NotFoundException("Урок не найден");

            LessonCompletion? lessonCompletion = await unitOfWork.LessonCompletions
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (lessonCompletion != null) return;

            lessonCompletion = new LessonCompletion()
            {
                User = user,
                Lesson = lesson,
                Completed = false,
                CompletionProgress = 0
            };

            await unitOfWork.LessonCompletions.AddAsync(lessonCompletion);
        }
    }
}
