using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class UserCourseLocalRolesInteractor(IUnitOfWork unitOfWork)
    {
        public async Task<Response<LocalRoleDto>> GetLocalRoleByUserCourse(int userId, int courseId)
        {
            try
            {
                var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(role => role.CourseId == courseId && role.UserId == userId);

                if (userCourseLocalRole == null)
                {
                    throw new NotFoundException("Пользовательская роль не найден");
                }

                await unitOfWork.UserCourseLocalRoles.Entry(userCourseLocalRole)
                    .Reference(role => role.LocalRole)
                    .LoadAsync();

                return new()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Локальная роль пользователя успешно обновлена",
                    Value = userCourseLocalRole.LocalRole.ToDto()
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
                    StatusCode = 500,
                    Message = "Не удалось обновить локальную роль пользователя",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> RequestReassignUserRoleAsync(int requesterUserId, int targetUserId, int courseId,
            int localRoleId)
        {
            try
            {
                var requesterUserLocalRole = await unitOfWork.UserCourseLocalRoles
                    .Include(role => role.LocalRole)
                    .Include(role => role.User)
                    .ThenInclude(role => role.Role)
                    .FirstOrDefaultAsync(userCourseLocalRole =>
                        userCourseLocalRole.CourseId == courseId &&
                        userCourseLocalRole.UserId == requesterUserId);

                var targetUserLocalRole = await unitOfWork.UserCourseLocalRoles
                    .Include(role => role.LocalRole)
                    .Include(role => role.User)
                    .FirstOrDefaultAsync(userCourseLocalRole =>
                        userCourseLocalRole.CourseId == courseId &&
                        userCourseLocalRole.UserId == targetUserId);

                if (requesterUserLocalRole == null || targetUserLocalRole == null)
                {
                    throw new NotFoundException("Локальная роль пользователя не найдена");
                }

                if (!(requesterUserLocalRole.User.Role.IsAdmin || requesterUserLocalRole.LocalRole.IsModerator))
                {
                    throw new AccessLevelException("Недостаточный уровень прав");
                }

                var localRole =
                    await unitOfWork.LocalRoles.FirstOrDefaultAsync(localRole => localRole.Id == localRoleId);

                if (localRole == null)
                {
                    throw new NotFoundException("Локальная роль не найдена");
                }

                var courseLocalRole = await unitOfWork.CourseLocalRoles.FirstOrDefaultAsync(courseLocalRole =>
                    courseLocalRole.CourseId == courseId && courseLocalRole.LocalRoleId == localRoleId
                );

                if (courseLocalRole == null)
                {
                    throw new NotFoundException("Локальная роль внутри курса не найдена");
                }

                await unitOfWork.CourseLocalRoles.Entry(courseLocalRole)
                    .Reference(role => role.Course)
                    .LoadAsync();

                if (!requesterUserLocalRole.User.Role.IsAdmin &&
                    requesterUserLocalRole.LocalRole.GetRolePriority() <
                    targetUserLocalRole.LocalRole.GetRolePriority())
                {
                    throw new AccessLevelException("Недостаточный уровень прав");
                }

                var userCourseLocalRole = new UserCourseLocalRole()
                {
                    User = targetUserLocalRole.User,
                    Course = courseLocalRole.Course,
                    LocalRole = courseLocalRole.LocalRole
                };

                unitOfWork.UserCourseLocalRoles.Remove(targetUserLocalRole);
                await unitOfWork.CommitAsync();

                await unitOfWork.UserCourseLocalRoles.AddAsync(userCourseLocalRole);
                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Локальная роль пользователя успешно обновлена"
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
                    Message = "Не удалось обновить локальную роль пользователя",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task CreateAsyncNoResponse(int userId, int courseId, int localRoleId)
        {
            var user = await unitOfWork.Users.FindAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("Пользователь не найден");
            }

            var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(userCourseLocalRole =>
                userCourseLocalRole.UserId == userId &&
                userCourseLocalRole.CourseId == courseId &&
                userCourseLocalRole.LocalRoleId == localRoleId
            );

            if (userCourseLocalRole != null)
            {
                throw new ValidationException("Для пользователя уже определены его локальные роли в курсе");
            }

            var courseLocalRole = await unitOfWork.CourseLocalRoles.FirstOrDefaultAsync(courseLocalRole =>
                courseLocalRole.CourseId == courseId &&
                courseLocalRole.LocalRoleId == localRoleId);

            if (courseLocalRole == null)
            {
                throw new NotFoundException("Локальная роль внутри курса не найдена");
            }

            await unitOfWork.CourseLocalRoles.Entry(courseLocalRole)
                .Reference(role => role.Course)
                .LoadAsync();

            await unitOfWork.CourseLocalRoles.Entry(courseLocalRole)
                .Reference(role => role.LocalRole)
                .LoadAsync();

            UserCourseLocalRole newUserCourseLocalRole = new UserCourseLocalRole()
            {
                User = user,
                Course = courseLocalRole.Course,
                LocalRole = courseLocalRole.LocalRole
            };

            await unitOfWork.UserCourseLocalRoles.AddAsync(newUserCourseLocalRole);
            await unitOfWork.CommitAsync();
        }

        public async Task RequestCreateAsyncNoResponse(int requesterUserId, int courseId, int localRoleId,
            params User[] users)
        {
            if (users.Length == 0)
            {
                throw new ArgumentNullException(nameof(users), "Users length was zero");
            }

            var courseLocalRole = await unitOfWork.CourseLocalRoles
                .FirstOrDefaultAsync(courseLocalRole =>
                    courseLocalRole.CourseId == courseId &&
                    courseLocalRole.LocalRoleId == localRoleId
                );

            if (courseLocalRole == null)
            {
                throw new NotFoundException("Присваемая локальная роль не найдена");
            }

            await unitOfWork.CourseLocalRoles.Entry(courseLocalRole)
                .Reference(role => role.LocalRole)
                .LoadAsync();

            await unitOfWork.CourseLocalRoles.Entry(courseLocalRole)
                .Reference(role => role.Course)
                .LoadAsync();


            var requesterLocalRole = await unitOfWork.UserCourseLocalRoles
                .Include(userCourseLocalRole => userCourseLocalRole.LocalRole)
                .FirstOrDefaultAsync(userCourseLocalRole =>
                    userCourseLocalRole.UserId == requesterUserId &&
                    userCourseLocalRole.CourseId == courseId);

            if (requesterLocalRole == null)
            {
                throw new NotFoundException("Локальная роль пользователя не найдена");
            }

            if (requesterLocalRole.LocalRole.GetRolePriority() < courseLocalRole.LocalRole.GetRolePriority())
            {
                throw new AccessLevelException("Приоритет вашей роли низкий");
            }

            List<UserCourseLocalRole> userCourseLocalRoles = new List<UserCourseLocalRole>();

            foreach (var user in users)
            {
                userCourseLocalRoles.Add(
                    new UserCourseLocalRole()
                    {
                        User = user,
                        Course = courseLocalRole.Course,
                        LocalRole = courseLocalRole.LocalRole
                    }
                );
            }

            await unitOfWork.UserCourseLocalRoles.AddRangeAsync(userCourseLocalRoles);
            await unitOfWork.CommitAsync();
        }

        public async Task RequestRemoveAsyncNoResponse(int requesterUserId, int targetUserId, int courseId)
        {
            var requesterCourseLocalRole = await unitOfWork.UserCourseLocalRoles
                .Include(userCourseLocalRole => userCourseLocalRole.LocalRole)
                .FirstOrDefaultAsync(courseLocalRole =>
                    courseLocalRole.UserId == requesterUserId &&
                    courseLocalRole.CourseId == courseId
                );

            if (requesterCourseLocalRole == null)
            {
                throw new NotFoundException("Ваша локальная роль не найдена");
            }

            var targetCourseLocalRole = await unitOfWork.UserCourseLocalRoles
                .Include(userCourseLocalRole => userCourseLocalRole.LocalRole)
                .FirstOrDefaultAsync(courseLocalRole =>
                    courseLocalRole.UserId == targetUserId &&
                    courseLocalRole.CourseId == courseId
                );

            if (targetCourseLocalRole == null)
            {
                throw new NotFoundException("Локальная роль пользователя не найдена");
            }

            if (!requesterCourseLocalRole.LocalRole.KickAccess ||
                requesterCourseLocalRole.LocalRole.GetRolePriority() <
                targetCourseLocalRole.LocalRole.GetRolePriority())
            {
                throw new AccessLevelException("Недостаточно прав для удаления локальной роли у пользователя");
            }

            unitOfWork.UserCourseLocalRoles.Remove(targetCourseLocalRole);
            await unitOfWork.CommitAsync();
        }

        public async Task RemoveAsyncNoResponse(int userId, int courseId)
        {
            var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(userCourseLocalRole =>
                userCourseLocalRole.UserId == userId &&
                userCourseLocalRole.CourseId == courseId
            );

            if (userCourseLocalRole == null)
            {
                throw new NotFoundException("Локальная роль пользователя не найдена");
            }

            unitOfWork.UserCourseLocalRoles.Remove(userCourseLocalRole);
        }
    }
}