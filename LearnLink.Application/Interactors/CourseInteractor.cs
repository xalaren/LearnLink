using LearnLink.Application.Helpers;
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
    public class CourseInteractor(
        IUnitOfWork unitOfWork,
        PermissionService permissionService,
        ModuleInteractor moduleInteractor,
        CourseLocalRoleInteractor courseLocalRoleInteractor,
        UserCourseLocalRolesInteractor userCourseLocalRoleInteractor)
    {

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

        public async Task<Response<ClientCourseDto?>> GetAnyCourseAsync(int userId, int courseId)
        {
            try
            {
                var course =
                    await unitOfWork.Courses.FirstOrDefaultAsync(course => course.Id == courseId) ??
                    throw new NotFoundException("Курс не найден");

                var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles
                    .Include(userCourseLocalRole => userCourseLocalRole.LocalRole)
                    .FirstOrDefaultAsync(u => u.CourseId == courseId && u.UserId == userId);

                if (userCourseLocalRole == null && course.IsPublic)
                {
                    return new()
                    {
                        Success = true,
                        Message = "Курс успешно получен",
                        Value = course.ToClientCourseDto(),
                    };
                }

                if (userCourseLocalRole == null)
                {
                    throw new NotFoundException("Не удалось определить роль пользователя");
                }

                var subscription = await unitOfWork.Subscriptions.FirstOrDefaultAsync(subscription => subscription.UserId == userId && subscription.CourseId == courseId);


                var localRole = userCourseLocalRole.LocalRole;

                if (!localRole.IsAdmin && !localRole.ViewAccess)
                {
                    throw new AccessLevelException("Курс недоступен пользователю");
                }

                var completion = await unitOfWork.CourseCompletions.FirstOrDefaultAsync(c => c.UserId == userId && c.CourseId == courseId);

                return new()
                {
                    Success = true,
                    Message = "Курс успешно получен",
                    Value = course.ToClientCourseDto(localRole, completion, subscription),
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

        public async Task<Response<DataPage<CourseDto[]>>> FindCoursesByTitle(string? searchTitle, DataPageHeader pageHeader)
        {
            try
            {
                var coursesQuery = unitOfWork.Courses.AsNoTracking().Where(course => course.IsPublic);

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

        public async Task<Response<DataPage<CourseDto[]>>> FindCoursesByTitleInUserCourses(int userId, string? title, bool isSubscription, bool isUnavailable, DataPageHeader pageHeader)
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
                        )
                        .Where(course => !course.IsUnavailable) :
                    unitOfWork.UserCreatedCourses
                        .AsNoTracking()
                        .Where(u => u.UserId == userId)
                        .Join(
                            unitOfWork.Courses.AsNoTracking(),
                            userCreatedCourse => userCreatedCourse.CourseId,
                            course => course.Id,
                            (userCreatedCourse, course) => course
                        )
                        .Where(course => (isUnavailable && course.IsUnavailable) || (!isUnavailable && !course.IsUnavailable));

                if (!string.IsNullOrWhiteSpace(title))
                {
                    coursesQuery = coursesQuery.Where(course => course.Title.ToLower().Contains(title.ToLower()));
                };


                var total = await coursesQuery.CountAsync();

                var courses = await coursesQuery
                    .OrderByDescending(course => course.CreationDate)
                    .Skip((pageHeader.PageNumber - 1) * pageHeader.PageSize)
                    .Take(pageHeader.PageSize)
                    .Select(course => course.ToDto())
                    .ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Курсы успешно получены",
                    Value = new DataPage<CourseDto[]>()
                    {
                        ItemsCount = total,
                        PageNumber = pageHeader.PageNumber,
                        PageSize = pageHeader.PageSize,
                        Values = courses
                    }
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

        public async Task<Response<DataPage<ParticipantDto[]>>> FindParticipantsAsync(int userId, int courseId, string? searchText, DataPageHeader pageHeader)
        {
            try
            {
                var viewPermission = await permissionService.GetPermissionAsync(userId: userId, courseId: courseId, toView: true);

                if (!viewPermission)
                {
                    throw new AccessLevelException("Доступ отклонен");
                }

                IEnumerable<UserCourseLocalRole> usersQuery = unitOfWork.UserCourseLocalRoles
                    .AsNoTracking()
                    .Include(userCourseLocalRole => userCourseLocalRole.User)
                    .Include(userCourseLocalRole => userCourseLocalRole.LocalRole)
                    .Where(u => u.CourseId == courseId);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    searchText = searchText.ToLower();
                    usersQuery = usersQuery.Where(u =>
                        u.User.Nickname.ToLower().Contains(searchText) ||
                        u.User.Lastname.ToLower().Contains(searchText) ||
                        u.User.Name.ToLower().Contains(searchText) ||
                        u.LocalRole.Name.ToLower().Contains(searchText) ||
                        u.LocalRole.Sign.ToLower().Contains(searchText));
                }

                int total = usersQuery.Count();

                usersQuery = usersQuery
                    .OrderBy(u => u.User.Lastname)
                    .ThenBy(u => u.User.Name)
                    .ThenBy(u => u.User.Nickname)
                    .Skip((pageHeader.PageNumber - 1) * pageHeader.PageSize)
                    .Take(pageHeader.PageSize);

                var courseUsers = usersQuery
                    .Select(u => new ParticipantDto(
                        Id: u.User.Id,
                        Nickname: u.User.Nickname,
                        Name: u.User.Name,
                        Lastname: u.User.Lastname,
                        AvatarUrl: u.User.AvatarFileName != null ?
                            DirectoryStore.GetRelativeDirectoryUrlToUserImages(u.User.Id) + u.User.AvatarFileName :
                            null,
                        LocalRole: u.LocalRole.ToDto()
                        ));

                var dataPage = new DataPage<ParticipantDto[]>()
                {
                    ItemsCount = total,
                    PageNumber = pageHeader.PageNumber,
                    PageSize = pageHeader.PageSize,
                    Values = courseUsers.ToArray()
                };

                return new()
                {
                    Success = true,
                    Message = "Подписчики успешно получены",
                    StatusCode = 200,
                    Value = dataPage
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

                await unitOfWork.Courses.AddAsync(course);
                await unitOfWork.UserCreatedCourses.AddAsync(userCreatedCourse);
                await unitOfWork.CourseCompletions.AddAsync(courseCompletion);
                await unitOfWork.CommitAsync();

                await courseLocalRoleInteractor.CreateDefaultLocalRoles(course.Id, RoleSignConstants.MODERATOR, RoleSignConstants.MEMBER);
                await AssignModeratorAsync(userId, course.Id);

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

        public async Task<Response> SetCourseUnavailableAsync(int userId, int courseId)
        {
            try
            {

                var course = await unitOfWork.Courses.FindAsync(courseId) ??
                    throw new NotFoundException("Курс не найден");

                var editPermission = await permissionService.GetPermissionAsync(userId: userId, courseId: courseId, toEdit: true);

                if (!editPermission)
                {
                    throw new AccessLevelException("Доступ отклонен");
                }

                course.IsUnavailable = true;
                course.IsPublic = false;

                unitOfWork.Courses.Update(course);

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    Message = "Курс успешно скрыт",
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
                    Message = "Не удалось скрыть курс",
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
        private async Task RemoveCourseAsyncNoResponse(int courseId, bool strictRemove)
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

        private async Task AssignModeratorAsync(int userId, int courseId)
        {
            var moderatorLocalRole = await unitOfWork.LocalRoles.FirstOrDefaultAsync(localRole => localRole.Sign == RoleSignConstants.MODERATOR);

            if (moderatorLocalRole == null)
            {
                throw new NotFoundException("Локальная роль модератора не найдена");
            }

            await userCourseLocalRoleInteractor.CreateAsyncNoResponse(userId, courseId, moderatorLocalRole.Id);
        }
    }
}
