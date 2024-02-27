using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class CourseInteractor(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Response<CourseDto?>> GetCourseAsync(int courseId)
        {
            try
            {
                var course = await unitOfWork.Courses.FirstOrDefaultAsync(course => course.Id == courseId);

                return course == null
                    ? throw new NotFoundException("Курс не найден")
                    : new()
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
                    InnerErrorMessages = [exception.Message]
                };
            }
        }

        public async Task<Response<CourseDto?>> GetAnyCourseAsync(int userId, int courseId)
        {
            try
            {
                var course =
                    await unitOfWork.Courses.FirstOrDefaultAsync(course => course.Id == courseId) ??
                    throw new NotFoundException("Курс не найден");

                var userCreatedCourse = await unitOfWork.UserCreatedCourses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId);

                var subscription = await unitOfWork.Subscriptions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId);

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
                    InnerErrorMessages = [exception.Message]
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response<DataPage<CourseDto[]>>> GetCoursesCreatedByUserAsync(int userId, DataPageHeader pageHeader)
        {
            try
            {
                var user = await unitOfWork.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(user => user.Id == userId) ?? throw new NotFoundException("Пользователь не найден");

                var courses = unitOfWork.UserCreatedCourses
                    .AsNoTracking()
                    .Where(u => u.UserId == userId)
                    .Join(
                        unitOfWork.Courses.AsNoTracking(),
                        userCreatedCourse => userCreatedCourse.CourseId,
                        course => course.Id,
                        (userCreatedCourse, course) => course
                    );

                var total = courses.Count();

                var result = await courses
                    .Skip((pageHeader.PageNumber - 1) * pageHeader.PageSize)
                    .Take(pageHeader.PageSize)
                    .Select(course => course.ToDto())
                    .ToArrayAsync();

                var dataPage = new DataPage<CourseDto[]>()
                {
                    PageNumber = pageHeader.PageNumber,
                    PageSize = pageHeader.PageSize,
                    ItemsCount = total,
                    Values = result
                };

                return new()
                {
                    Success = true,
                    Message = "Курсы пользователя успешно получены",
                    Value = dataPage,
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response<DataPage<CourseDto[]>>> GetSubscribedCoursesAsync(int userId, DataPageHeader pageHeader)
        {
            try
            {
                var user = await unitOfWork.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId) ?? throw new NotFoundException("Пользователь не найден");

                var subscriptions = await unitOfWork
                    .Subscriptions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.UserId == userId);

                var courses = unitOfWork.Subscriptions
                    .AsNoTracking()
                    .Where(s => s.UserId == userId)
                    .Join(
                        unitOfWork.Courses.AsNoTracking(),
                        sub => sub.CourseId,
                        course => course.Id,
                        (sub, course) => course
                    );

                var total = courses.Count();

                var coursesResult = await courses
                    .Skip((pageHeader.PageNumber - 1) * pageHeader.PageSize)
                    .Take(pageHeader.PageSize)
                    .Select(course => course.ToDto())
                    .ToArrayAsync();

                var dataPage = new DataPage<CourseDto[]>()
                {
                    PageNumber = pageHeader.PageNumber,
                    PageSize = pageHeader.PageSize,
                    ItemsCount = total,
                    Values = coursesResult
                };

                return new()
                {
                    Success = true,
                    Message = "Курсы пользователя успешно получены",
                    Value = dataPage,
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response<DataPage<CourseDto[]>>> GetUnavailableUserCoursesAsync(int userId, DataPageHeader pageHeader)
        {
            try
            {
                var user = await unitOfWork.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(user => user.Id == userId) ?? throw new NotFoundException("Пользователь не найден");

                var courses = unitOfWork.UserCreatedCourses
                    .AsNoTracking()
                    .Where(u => u.UserId == userId)
                    .Join(
                        unitOfWork.Courses.AsNoTracking(),
                        userCreatedCourse => userCreatedCourse.CourseId,
                        course => course.Id,
                        (userCreatedCourse, course) => course
                    )
                    .Where(c => c.IsUnavailable);

                var total = courses.Count();

                var result = await courses
                    .Skip((pageHeader.PageNumber - 1) * pageHeader.PageSize)
                    .Take(pageHeader.PageSize)
                    .Select(course => course.ToDto())
                    .ToArrayAsync();

                var dataPage = new DataPage<CourseDto[]>()
                {
                    PageNumber = pageHeader.PageNumber,
                    PageSize = pageHeader.PageSize,
                    ItemsCount = total,
                    Values = result
                };

                return new()
                {
                    Success = true,
                    Message = "Курсы пользователя успешно получены",
                    Value = dataPage,
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response> CreateCourseAsync(int userId, CourseDto courseDto)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(courseDto, "CourseDto was null");

                var user = await unitOfWork.Users.FindAsync(userId) ??
                    throw new NotFoundException("Пользователь не найден");

                var course = courseDto.ToEntity();

                if (course.IsUnavailable)
                {
                    course.IsPublic = false;
                }

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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response<DataPage<CourseDto[]>>> GetPublicCoursesAsync(DataPageHeader pageHeader)
        {
            try
            {
                var total = await unitOfWork.Courses
                    .AsNoTracking()
                    .CountAsync();

                var courses = await unitOfWork.Courses
                    .AsNoTracking()
                    .Where(c => c.IsPublic)
                    .Skip((pageHeader.PageNumber - 1) * pageHeader.PageSize)
                    .Take(pageHeader.PageSize)
                    .Select(c => c.ToDto())
                    .ToArrayAsync();

                var dataPage = new DataPage<CourseDto[]>()
                {
                    PageNumber = pageHeader.PageNumber,
                    PageSize = pageHeader.PageSize,
                    ItemsCount = total,
                    Values = courses
                };

                return new()
                {
                    Success = true,
                    Message = "Курсы успешно получены",
                    Value = dataPage,
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response<CourseDto[]>> FindCoursesByTitle(string searchTitle, bool findPublic, bool findPrivate, bool findUnavailable)
        {
            try
            {
                var coursesQuery = unitOfWork.Courses
                                 .AsNoTracking()
                                 .Where(course =>
                                     (findPublic && course.IsPublic) ||
                                     (findPrivate && !course.IsPublic && !course.IsUnavailable) ||
                                     (findUnavailable && course.IsUnavailable))
                                 .Where(course => course.Title.ToLower().Contains(searchTitle.ToLower()))
                                 .Select(course => course.ToDto());

                var courses = await coursesQuery.ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Курсы успешно получены",
                    Value = courses
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response> UpdateCourseAsync(CourseDto courseDto)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(courseDto, nameof(courseDto));

                var course = await unitOfWork.Courses.FindAsync(courseDto.Id) ??
                    throw new NotFoundException("Курс не найден");

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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response> RemoveCourseAsync(int courseId)
        {
            try
            {
                var course = await unitOfWork.Courses.FindAsync(courseId) ??
                    throw new NotFoundException("Курс не найден");

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
                    InnerErrorMessages = [exception.Message],
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
                    InnerErrorMessages = [exception.Message],
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }
    }
}
