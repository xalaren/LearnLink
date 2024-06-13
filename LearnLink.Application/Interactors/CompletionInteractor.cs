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

        public async Task CreateModuleCompletion(int userId, int courseId, int moduleId)
        {
            User? user = await unitOfWork.Users.FindAsync(userId) ?? throw new NotFoundException("Пользователь не найден");
            Course? course = await unitOfWork.Courses.FindAsync(courseId) ?? throw new NotFoundException("Курс не найден");
            Module? module = await unitOfWork.Modules.FindAsync(moduleId) ?? throw new NotFoundException("Модуль не найден");


            ModuleCompletion? moduleCompletion = await unitOfWork.ModuleCompletions
                .AsNoTracking()
                .FirstOrDefaultAsync(moduleCompletion => moduleCompletion.UserId == userId && moduleCompletion.ModuleId == moduleId);

            if (moduleCompletion != null) return;

            moduleCompletion = new ModuleCompletion()
            {
                User = user,
                Module = module,
                Course = course,
                Completed = false,
                CompletionProgress = 0,
            };

            await unitOfWork.ModuleCompletions.AddAsync(moduleCompletion);
            await unitOfWork.CommitAsync();
        }

        public async Task CreateLessonCompletion(int userId, int moduleId, int lessonId)
        {
            User? user = await unitOfWork.Users.FindAsync(userId) ?? throw new NotFoundException("Пользователь не найден");
            Module? module = await unitOfWork.Modules.FindAsync(moduleId) ?? throw new NotFoundException("Модуль не найден");
            Lesson? lesson = await unitOfWork.Lessons.FindAsync(lessonId) ?? throw new NotFoundException("Урок не найден");

            LessonCompletion? lessonCompletion = await unitOfWork.LessonCompletions
                .AsNoTracking()
                .FirstOrDefaultAsync(lessonCompletion => 
                    lessonCompletion.UserId == userId && 
                    lessonCompletion.ModuleId == moduleId && 
                    lessonCompletion.LessonId == lessonId);

            if (lessonCompletion != null) return;

            lessonCompletion = new LessonCompletion()
            {
                User = user,
                Module = module,
                Lesson = lesson,
                Completed = false,
                CompletionProgress = 0
            };

            await unitOfWork.LessonCompletions.AddAsync(lessonCompletion);
            await unitOfWork.CommitAsync();
        }

        public async Task<Response> ChangeLessonCompleted(int userId, int courseId, int moduleId, int lessonId, bool completed)
        {
            try
            {
                var lessonCompletion = await unitOfWork.LessonCompletions.FirstOrDefaultAsync(completion =>
                    completion.UserId == userId &&
                    completion.LessonId == lessonId);

                NotFoundException.ThrowIfNotFound(lessonCompletion, "Прогресс урока не найден");

                lessonCompletion.Completed = completed;
                unitOfWork.LessonCompletions.Update(lessonCompletion);
                await unitOfWork.CommitAsync();

                await RefreshModuleCompletionByLessonCompletions(userId, moduleId);
                await RefreshCourseCompletionByModuleCompletions(userId, courseId);

                return new()
                {
                    Success = true,
                    Message = $"Прогресс урока успешно записан",
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось пройти урок",
                    InnerErrorMessages = [exception.Message]
                };
            }
        }

        public async Task RefreshModuleCompletionByLessonCompletions(int userId, int moduleId)
        {
            var moduleCompletion = await unitOfWork.ModuleCompletions.FirstOrDefaultAsync(completion =>
                completion.UserId == userId &&
                completion.ModuleId == moduleId);

            NotFoundException.ThrowIfNotFound(moduleCompletion, "Прогресс модуля не найден");

            var lessonCompletions = await unitOfWork.LessonCompletions.Where(completion =>
                    completion.UserId == userId &&
                    completion.ModuleId == moduleId)
                .ToArrayAsync();

            int maxCount = lessonCompletions.Length;

            int completedCount = lessonCompletions.Count(completion => completion.Completed);

            if (maxCount == 0) return;

            int increment = (int)Math.Round((double)(Completion.MAX_COMPLETION_VALUE / maxCount), 0);

            moduleCompletion.CompletionProgress = maxCount == completedCount
                ? Completion.MAX_COMPLETION_VALUE
                : completedCount * increment;

            unitOfWork.ModuleCompletions.Update(moduleCompletion);
            await unitOfWork.CommitAsync();
        }

        public async Task RefreshCourseCompletionByModuleCompletions(int userId, int courseId)
        {
            var courseCompletion = await unitOfWork.CourseCompletions.FirstOrDefaultAsync(completion =>
                completion.UserId == userId &&
                completion.CourseId == courseId);

            NotFoundException.ThrowIfNotFound(courseCompletion, "Прогресс курса не найден");

            var moduleCompletions = await unitOfWork.ModuleCompletions.Where(completion =>
                    completion.UserId == userId &&
                    completion.CourseId == courseId)
                .ToArrayAsync();

            int maxCount = moduleCompletions.Length;

            int completedCount = moduleCompletions.Count(completion => completion.Completed);

            if (maxCount == 0) return;

            int increment = (int)Math.Round((double)(Completion.MAX_COMPLETION_VALUE / maxCount), 0);

            courseCompletion.CompletionProgress = maxCount == completedCount
                ? Completion.MAX_COMPLETION_VALUE
                : completedCount * increment;

            unitOfWork.CourseCompletions.Update(courseCompletion);
            await unitOfWork.CommitAsync();
        }
    }
}
