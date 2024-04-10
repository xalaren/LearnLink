using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class LessonInteractor(IUnitOfWork unitOfWork, ContentInteractor contentInteractor)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly ContentInteractor contentInteractor = contentInteractor;

        public async Task<Response<LessonDto[]>> GetAllLessonsAsync()
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

        public async Task<Response<LessonDto>> GetLessonAsync(int lessonId)
        {
            try
            {
                var lesson = await unitOfWork.Lessons.FindAsync(lessonId);

                return new Response<LessonDto>()
                {
                    Success = true,
                    StatusCode = 200,
                    Value = lesson?.ToDto(),
                    Message = "Урок успешно получен"
                };
            }
            catch (CustomException exception)
            {
                return new Response<LessonDto>()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response<LessonDto>()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить урок",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> CreateLessonAsync(int courseId, int moduleId, LessonDto lessonDto)
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
                    Message = "Урок успешно создан",
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

        public async Task<Response> UpdateLessonAsync(LessonDto lessonDto)      
        {
            try
            {
                if (lessonDto == null)
                {
                    throw new ArgumentNullException(nameof(lessonDto), "ModuleDto was null");
                }

                var lesson = await unitOfWork.Lessons.FindAsync(lessonDto.Id);

                if (lesson == null)
                {
                    throw new NotFoundException("Урко не найден");
                }

                lesson.Assign(lessonDto);
                unitOfWork.Lessons.Update(lesson);

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Урок успешно изменен"
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
                    Message = "Не удалось изменить урок",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> RemoveLessonAsync(int lessonId)
        {
            try
            {
                await RemoveLessonAsyncNoResponse(lessonId);

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Урок успешно удален",
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
                    Message = "Не удалось удалить урок",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task RemoveLessonAsyncNoResponse(int lessonId)
        {
            var lesson = await unitOfWork.Lessons.FindAsync(lessonId);

            if (lesson == null) return;

            var lessonContents = await unitOfWork.LessonContents
                .Where(lessonContent => lessonContent.LessonId == lessonId)
                .ToListAsync();

            foreach(var lessonContent in lessonContents)
            {
                await contentInteractor.RemoveContentAsyncNoResponse(lessonContent.LessonId);
            }

            var completions = unitOfWork.LessonCompletions.Where(lessonCompletion => lessonCompletion.LessonId == lessonId);

            unitOfWork.LessonCompletions.RemoveRange(completions);
            unitOfWork.Lessons.Remove(lesson);
        }

    }

}
