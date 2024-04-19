using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Constants;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class CourseInteractor(IUnitOfWork unitOfWork, PermissionService permissionService, ModuleInteractor moduleInteractor)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly PermissionService permissionService = permissionService;
        private readonly ModuleInteractor moduleInteractor = moduleInteractor;

        //Get, Find methods

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

                if (!course.IsPublic && (userCreatedCourse == null || subscription == null))
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

        public async Task<Response<CourseDto[]>> GetCoursesCreatedByUserAsync(int userId)
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
                    .OrderByDescending(course => course.CreationDate)
                    .Select(course => course.ToDto())
                    .ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Курсы пользователя успешно получены",
                    Value = result,
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

        public async Task<Response<CourseDto[]>> GetSubscribedCoursesAsync(int userId)
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

                var coursesResult = await courses
                    .OrderByDescending(course => course.CreationDate)
                    .Select(course => course.ToDto())
                    .ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Курсы пользователя успешно получены",
                    Value = coursesResult,
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

        public async Task<Response<CourseDto[]>> GetUnavailableUserCoursesAsync(int userId)
        {
            try
            {
                var user = await unitOfWork.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(user => user.Id == userId) ?? throw new NotFoundException("Пользователь не найден");

                var courses = await unitOfWork.UserCreatedCourses
                    .AsNoTracking()
                    .Where(u => u.UserId == userId)
                    .Join(
                        unitOfWork.Courses.AsNoTracking(),
                        userCreatedCourse => userCreatedCourse.CourseId,
                        course => course.Id,
                        (userCreatedCourse, course) => course
                    )
                    .Where(c => c.IsUnavailable)
                    .OrderByDescending(course => course.CreationDate)
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
                    .OrderByDescending(c => c.CreationDate)
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

        public async Task<Response<DataPage<CourseDto[]>>> FindCoursesByTitle(string searchTitle, DataPageHeader pageHeader)
        {
            try
            {
                var coursesQuery = unitOfWork.Courses.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(searchTitle))
                {
                    coursesQuery = coursesQuery.Where(course => course.Title.ToLower().Contains(searchTitle.ToLower()));
                };

                coursesQuery = coursesQuery.OrderByDescending(course => course.CreationDate);

                var total = await coursesQuery.CountAsync();
                var courses = await coursesQuery
                    .Skip((pageHeader.PageNumber - 1) * pageHeader.PageSize)
                    .Take(pageHeader.PageSize)
                    .Select(course => course.ToDto())
                    .ToArrayAsync();

                var dataPage = new DataPage<CourseDto[]>()
                {
                    ItemsCount = total,
                    PageSize = pageHeader.PageSize,
                    PageNumber = pageHeader.PageNumber,
                    Values = courses
                };

                return new()
                {
                    Success = true,
                    Message = "Курсы успешно получены",
                    Value = dataPage
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

        public async Task<Response<CourseDto[]>> FindCoursesByTitleInUserCourses(int userId, string searchTitle, bool isSubscription, bool isUnavailable)
        {
            try
            {
                var user = await unitOfWork.Users
                   .AsNoTracking()
                   .FirstOrDefaultAsync(user => user.Id == userId) ?? throw new NotFoundException("Пользователь не найден");

                var coursesQuery = isSubscription ?
                    unitOfWork.Subscriptions
                        .AsNoTracking()
                        .Where(u => u.UserId == userId)
                        .Join(
                            unitOfWork.Courses.AsNoTracking(),
                            sub => sub.CourseId,
                            course => course.Id,
                            (sub, course) => course
                        ) :
                    unitOfWork.UserCreatedCourses
                        .AsNoTracking()
                        .Where(u => u.UserId == userId)
                        .Join(
                            unitOfWork.Courses.AsNoTracking(),
                            userCreatedCourse => userCreatedCourse.CourseId,
                            course => course.Id,
                            (userCreatedCourse, course) => course
                        )
                        .Where(course =>
                            (isUnavailable && course.IsUnavailable) ||
                            (!isUnavailable && !course.IsPublic && !course.IsUnavailable));

                var filteredCourses = coursesQuery
                    .Where(course => course.Title.ToLower().Contains(searchTitle.ToLower()))
                    .OrderByDescending(course => course.CreationDate);

                var total = await filteredCourses.CountAsync();

                var courses = await filteredCourses
                    .Select(course => course.ToDto())
                    .ToArrayAsync();

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

        public async Task<Response<CourseUserDto[]>> GetSubscribers(int userId, int courseId)
        {
            try
            {
                var viewPermission = await permissionService.GetPermissionAsync(userId: userId, courseId: courseId, toView: true);

                if (!viewPermission)
                {
                    throw new AccessLevelException("Доступ отклонен");
                }

                var subscriptions = unitOfWork.Subscriptions
                    .AsNoTracking()
                    .Where(sub => sub.CourseId == courseId);

                var users = subscriptions.Join(
                        unitOfWork.Users.AsNoTracking(),
                        sub => sub.UserId,
                        user => user.Id,
                        (sub, user) => user
                    );

                var userRoles = subscriptions.Join(
                    unitOfWork.UserCourseLocalRoles.AsNoTracking(),
                    sub => sub.UserId,
                    userRole => userRole.UserId,
                    (sub, userRole) => userRole)
                    .Include(userRole => userRole.LocalRole);

                var courseUserDtos = await subscriptions
                        .Join(
                            users,
                            sub => sub.UserId,
                            user => user.Id,
                            (sub, user) => new { Subscription = sub, User = user })
                        .Join(
                            userRoles,
                            combined => combined.Subscription.UserId,
                            localRole => localRole.UserId,
                            (combined, userRole) => new CourseUserDto(
                                combined.User.Id,
                                combined.User.Nickname,
                                combined.User.Name,
                                combined.User.Lastname,
                                userRole.LocalRole.Name,
                                combined.Subscription.StartDate))
                        .ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Подписчики успешно получены",
                    StatusCode = 200,
                    Value = courseUserDtos
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                    StatusCode = exception.StatusCode
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Не удалось получить подписчиков курса",
                    InnerErrorMessages = [exception.Message],
                    StatusCode = 500
                };
            }
        }

        //Create, Edit, Remove methods

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

                var courseCompletion = new CourseCompletion()
                {
                    Course = course,
                    User = user,
                    Completed = false,
                    CompletionProgress = 0,
                };

                var role = await unitOfWork.LocalRoles.FirstOrDefaultAsync(x => x.Sign == RoleSignConstants.MODERATOR);

                if (role == null)
                {
                    throw new NotFoundException("Не найдена роль модератора. Обратитесь к администратору или разработчику.");
                }

                var userCourseRole = new UserCourseLocalRole()
                {
                    Course = course,
                    LocalRole = role,
                    User = user,
                };

                await unitOfWork.Courses.AddAsync(course);
                await unitOfWork.UserCreatedCourses.AddAsync(userCreatedCourse);
                await unitOfWork.UserCourseLocalRoles.AddAsync(userCourseRole);
                await unitOfWork.CourseCompletions.AddAsync(courseCompletion);

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

        public async Task<Response> UpdateCourseAsync(int userId, CourseDto courseDto)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(courseDto, nameof(courseDto));

                var course = await unitOfWork.Courses.FindAsync(courseDto.Id) ??
                    throw new NotFoundException("Курс не найден");

                var editPermission = await permissionService.GetPermissionAsync(userId: userId, courseId: courseDto.Id, toEdit: true);

                if (!editPermission)
                {
                    throw new AccessLevelException("Доступ отклонен");
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response> RemoveCourseAsync(int userId, int courseId)
        {
            try
            {
                var removePermission = await permissionService.GetPermissionAsync(userId: userId, courseId: courseId, toRemove: true);

                if (!removePermission)
                {
                    throw new AccessLevelException("Доступ отклонен");
                }

                await RemoveCourseAsyncNoResponse(courseId, true);

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

        public async Task<Response<LocalRoleDto?>> GetUserLocalRole(int userId, int courseId)
        {
            try
            {
                var userLocalRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);

                if (userLocalRole == null)
                {
                    throw new NotFoundException("Локальная роль пользователя в данном курсе не найдена");
                }

                await unitOfWork.UserCourseLocalRoles.Entry(userLocalRole)
                    .Reference(u => u.LocalRole)
                    .LoadAsync();

                return new()
                {
                    Success = true,
                    Message = "Локальная роль пользователя получена успешно",
                    Value = userLocalRole.LocalRole.ToDto()
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
                    Message = "Не удалось получить роль пользователя",
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task RemoveCourseAsyncNoResponse(int courseId, bool strictRemove)
        {
            var course = await unitOfWork.Courses.FindAsync(courseId) ??
                    throw new NotFoundException("Курс не найден");

            var modules = await unitOfWork.CourseModules
                .Where(courseModule => courseModule.CourseId == courseId)
                .ToListAsync();

            foreach (var module in modules)
            {
                await moduleInteractor.RemoveModuleAsyncNoResponse(module.ModuleId, false);
            }

            unitOfWork.Courses.Remove(course);
        }
    }
}
