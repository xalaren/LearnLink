using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class CourseInteractor
    {
        private readonly IUnitOfWork unitOfWork;

        public CourseInteractor(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response<CourseDto?>> GetCourseAsync(int courseId)
        {
            try
            {
                var course = await unitOfWork.Courses.FirstOrDefaultAsync(course => course.Id == courseId);

                if (course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                return new()
                {
                    Success = true,
                    Message = "Курс успешно получен",
                    Value = course.ToDto(),
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
                    Message = "Не удалось получить курс",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response<CourseDto?>> GetAnyCourseAsync(int userId, int courseId)
        {
            try
            {
                var course = await unitOfWork.Courses.FirstOrDefaultAsync(course => course.Id == courseId);

                if (course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                var userCreatedCourse = await unitOfWork.UserCreatedCourses.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId);
                var subscription = await unitOfWork.Subscriptions.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId);

                if (!course.IsPublic && userCreatedCourse == null && subscription == null)
                {
                    throw new AccessLevelException("Курс недоступен пользователю");
                }

                return new()
                {
                    Success = true,
                    Message = "Курс успешно получен",
                    Value = course.ToDto(),
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
                    Message = "Не удалось получить курс",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response<CourseDto[]>> GetAllAsync()
        {
            try
            {

                var courses = await unitOfWork.Courses.AsNoTracking().Select(course => course.ToDto()).ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Курсы успешно получены",
                    Value = courses,
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
                    Message = "Не удалось получить курсы",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response<CourseDto[]>> GetCoursesCreatedByUserAsync(int userId)
        {
            try
            {
                var user = await unitOfWork.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == userId);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                var courses = await unitOfWork.UserCreatedCourses
                    .AsNoTracking()
                    .Where(u => u.UserId == userId)
                    .Join(
                        unitOfWork.Courses.AsNoTracking(),
                        userCreatedCourse => userCreatedCourse.CourseId,
                        course => course.Id,
                        (userCreatedCourse, course) => course
                    )
                    .Select(course => course.ToDto())
                    .ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Курсы пользователя успешно получены",
                    Value = courses,
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
                    Message = "Не удалось получить курсы",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response<CourseDto[]>> GetSubscribedCoursesAsync(int userId)
        {
            try
            {
                var user = await unitOfWork.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                var subscriptions = await unitOfWork.Subscriptions.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId);

                var courses = await unitOfWork.Subscriptions
                    .AsNoTracking()
                    .Where(s => s.UserId == userId)
                    .Join(
                        unitOfWork.Courses.AsNoTracking(),
                        sub => sub.CourseId,
                        course => course.Id,
                        (sub, course) => course
                    )
                    .Select(course => course.ToDto())
                    .ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Курсы пользователя успешно получены",
                    Value = courses,
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
                    Message = "Не удалось получить курсы",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> CreateCourseAsync(int userId, CourseDto courseDto)
        {
            try
            {
                if (courseDto == null)
                {
                    throw new ArgumentNullException("CourseDto was null");
                }

                var user = await unitOfWork.Users.FindAsync(userId);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                var course = courseDto.ToEntity();

                var userCreatedCourse = new UserCreatedCourse()
                {
                    User = user,
                    Course = course,
                };


                await unitOfWork.Courses.AddAsync(course);
                await unitOfWork.UserCreatedCourses.AddAsync(userCreatedCourse);

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    Message = "Курс успешно создан",
                };
            }
            catch (CustomException exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = "Не удалось создать курс",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response<CourseDto[]>> GetPublicCoursesAsync()
        {
            try
            {
                var courses = await unitOfWork.Courses
                    .AsNoTracking()
                    .Where(c => c.IsPublic)
                    .Select(c => c.ToDto())
                    .ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Курсы успешно получены",
                    Value = courses,
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
                    Message = "Не удалось получить курсы",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> UpdateCourseAsync(CourseDto courseDto)
        {
            try
            {
                if (courseDto == null)
                {
                    throw new ArgumentNullException(nameof(courseDto), "CourseDto was null");
                }

                var course = await unitOfWork.Courses.FindAsync(courseDto.Id);

                if (course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                course = course.Assign(courseDto);

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    Message = "Курс успешно изменен",
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
                    Message = "Не удалось изменить курс",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> RemoveCourseAsync(int courseId)
        {
            try
            {
                var course = await unitOfWork.Courses.FindAsync(courseId);

                if (course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                unitOfWork.Courses.Remove(course);

                var modules = unitOfWork.CourseModules
                    .Where(m => m.CourseId == courseId)
                    .Join(
                        unitOfWork.Modules,
                        courseModule => courseModule.ModuleId,
                        module => module.Id,
                        (courseModule, module) => module
                    );

                unitOfWork.Modules.RemoveRange(modules);

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    Message = "Курс успешно удален",
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
                    Message = "Не удалось удалить курс",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response<bool>> IsCreator(int userId, int courseId)
        {
            try
            {
                var userCreatedCourse = await unitOfWork.UserCreatedCourses.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId);

                if (userCreatedCourse == null)
                {
                    return new()
                    {
                        Success = true,
                        Message = "Пользователь не является создателем",
                        Value = false,
                    };
                }

                return new()
                {
                    Success = true,
                    Message = "Пользователь является создателем",
                    Value = true,
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
                    Message = "Не удалось получить статус пользователя",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response<bool>> IsSubscriber(int userId, int courseId)
        {
            try
            {
                var userCreatedCourse = await unitOfWork.Subscriptions.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId);

                if (userCreatedCourse == null)
                {
                    return new()
                    {
                        Success = true,
                        Message = "Пользователь не является подписчиком",
                        Value = false,
                    };
                }

                return new()
                {
                    Success = true,
                    Message = "Пользователь является подписчиком",
                    Value = true,
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
                    Message = "Не удалось получить статус пользователя",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }
    }
}