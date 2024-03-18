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
    public class SubscriptionInteractor
    {
        private readonly IUnitOfWork unitOfWork;

        public SubscriptionInteractor(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response> InviteAsync(int userId, int courseId, string localRoleSign, params int[] userIdentifiers)
        {
            try
            {
                var course = await unitOfWork.Courses.FindAsync(courseId) ?? 
                    throw new NotFoundException("Курс не найден");

                var user = await unitOfWork.Users.FindAsync(userId) ?? throw new NotFoundException("Пользователь не найден");

                var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles
                    .FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId) ??
                        throw new NotFoundException("Не удалось определить локальную роль пользователя");

                await unitOfWork.UserCourseLocalRoles.Entry(userCourseLocalRole)
                    .Reference(u => u.LocalRole)
                    .LoadAsync();

                if(!userCourseLocalRole.LocalRole.InviteAccess)
                {
                    throw new AccessLevelException("Пользователь не может пригласить других участников");
                }

                var users = unitOfWork.Users.Where(u => userIdentifiers.Contains(u.Id));

                var subscriptions = users.Select(user => new Subscription()
                {
                    User = user,
                    Course = course,
                    StartDate = DateTime.Now.ToUniversalTime(),
                });

                var role = await unitOfWork.LocalRoles.FirstOrDefaultAsync(l => l.Sign == localRoleSign);

                if (role == null)
                {
                    throw new NotFoundException("Локальная роль получена неверно, либо не найдена");
                }

                if(userCourseLocalRole.LocalRole.GetRolePriority() < role.GetRolePriority())
                {
                    throw new AccessLevelException("Приоритет вашей роли недопустим для назначения данной роли пользователям");
                }

                var userLocalRoles = users.Select(user => new UserCourseLocalRole()
                {
                    User = user,
                    Course = course,
                    LocalRole = role
                });

                await unitOfWork.Subscriptions.AddRangeAsync(subscriptions);
                await unitOfWork.UserCourseLocalRoles.AddRangeAsync(userLocalRoles);
                var count = users.Count();
                course.SubscribersCount += count;

                unitOfWork.Courses.Update(course);

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    Message = "Запись прошла успешно",
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
                    Message = "Не удалось записаться на курс",
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response> SubscribeAsync(SubscriptionDto subscriptionDto)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(subscriptionDto, nameof(subscriptionDto));

                var user = await unitOfWork.Users.FindAsync(subscriptionDto.UserId) ??
                    throw new NotFoundException("Пользователь не найден");

                var course = await unitOfWork.Courses.FindAsync(subscriptionDto.CourseId) ??
                    throw new NotFoundException("Курс не найден");

                var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles
                   .FirstOrDefaultAsync(u => u.UserId == user.Id && u.CourseId == course.Id);

                if (userCourseLocalRole != null)
                {
                    throw new AccessLevelException("Пользователь уже является участником этого курса");
                }

                var role = await unitOfWork.LocalRoles.FirstOrDefaultAsync(role => string.Equals(role.Sign, RoleSignConstants.MEMBER));

                if (role == null)
                {
                    throw new NotFoundException("Локальная роль получена неверно, либо не найдена");
                }

                course.SubscribersCount++;

                var subscriptionEntity = subscriptionDto.ToEntity();

                var newUserCourseLocalRole = new UserCourseLocalRole()
                {
                    Course = course,
                    User = user,
                    LocalRole = role,
                };

                subscriptionEntity.User = user;
                subscriptionEntity.Course = course;

                await unitOfWork.Subscriptions.AddAsync(subscriptionEntity);
                await unitOfWork.UserCourseLocalRoles.AddAsync(newUserCourseLocalRole);

                unitOfWork.Courses.Update(course);

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    Message = "Запись прошла успешно",
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
                    Message = "Не удалось записаться на курс",
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response> Unsubscribe(int userId, int courseId)
        {
            try
            {
                var subscription = await unitOfWork.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.CourseId == courseId);

                var course = await unitOfWork.Courses.FindAsync(courseId);

                if (course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                course.SubscribersCount--;

                if (subscription == null)
                {
                    throw new NotFoundException("Подписка не найдена");
                }

                unitOfWork.Courses.Update(course);
                unitOfWork.Subscriptions.Remove(subscription);
                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    Message = "Отписка прошла успешно",
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
                    Message = "Не удалось отписаться от курса",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> KickUserAsync(int requesterUserId, int targetUserId, int courseId)
        {
            try
            {
                User requester = await unitOfWork.Users.FindAsync(requesterUserId) ??
                    throw new NotFoundException("Пользователь, запросивший ислючение, не найден");

                User target = await unitOfWork.Users.FindAsync(targetUserId) ??
                    throw new NotFoundException("Исключаемый пользователь не найден");

                Subscription subscription = await unitOfWork.Subscriptions.FirstOrDefaultAsync(s => s.UserId == targetUserId && s.CourseId == courseId) ??
                    throw new NotFoundException("Подписка не найдена");

                Course course = await unitOfWork.Courses.FindAsync(courseId) ??
                    throw new NotFoundException("Курс не найден");

                await unitOfWork.Users.Entry(requester)
                    .Reference(u => u.Role)
                    .LoadAsync();

                await unitOfWork.Users.Entry(target)
                    .Reference(u => u.Role)
                    .LoadAsync();

                UserCourseLocalRole requesterCourseLocalRole = await unitOfWork.UserCourseLocalRoles
                    .FirstOrDefaultAsync(x => x.UserId == requesterUserId && x.CourseId == courseId) ??
                        throw new NotFoundException("Роль пользователя-запросителя не определена");

                UserCourseLocalRole targetCourseLocalRole = await unitOfWork.UserCourseLocalRoles
                    .FirstOrDefaultAsync(x => x.UserId == targetUserId && x.CourseId == courseId) ??
                        throw new NotFoundException("Роль исключаемого пользователя не определена");

                if (!requester.Role.IsAdmin &&
                   (!requesterCourseLocalRole.LocalRole.KickAccess || 
                     requesterCourseLocalRole.LocalRole.GetRolePriority() < targetCourseLocalRole.LocalRole.GetRolePriority()))
                {
                    throw new AccessLevelException("Невозможно исключить данного пользователя");
                }

                if (subscription == null)
                {
                    throw new NotFoundException("Подписка не найдена");
                }

                unitOfWork.Subscriptions.Remove(subscription);

                course.SubscribersCount--;
                unitOfWork.Courses.Update(course);

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    Message = "Исключение прошло успешно",
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
                    Message = "Не удалось отписаться от курса",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        } 

    }
}
