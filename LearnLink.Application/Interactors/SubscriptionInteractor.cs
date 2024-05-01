using System.Runtime.InteropServices;
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
        private readonly CompletionInteractor completionInteractor;

        public SubscriptionInteractor(IUnitOfWork unitOfWork, CompletionInteractor completionInteractor)
        {
            this.unitOfWork = unitOfWork;
            this.completionInteractor = completionInteractor;
        }

        public async Task<Response<SubscriptionDto[]>> GetAllAsync()
        {
            try
            {
                var subscriptions = await unitOfWork.Subscriptions.Select(sub => sub.ToDto()).ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Подписки успешно получены",
                    StatusCode = 200,
                    Value = subscriptions,
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                    StatusCode = exception.StatusCode,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Не удалось получить курсы",
                    StatusCode = 500,
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response> InviteAsync(int userId, int courseId, string? localRoleSign, params int[] userIdentifiers)
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

                if (!userCourseLocalRole.LocalRole.InviteAccess)
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


                var role = await unitOfWork.LocalRoles.FirstOrDefaultAsync(l => (localRoleSign != null && l.Sign == localRoleSign) || l.Sign == RoleSignConstants.MEMBER);

                if (role == null)
                {
                    throw new NotFoundException("Локальная роль получена неверно, либо не найдена");
                }

                if (userCourseLocalRole.LocalRole.GetRolePriority() < role.GetRolePriority())
                {
                    throw new AccessLevelException("Приоритет вашей роли недопустим для назначения данной роли пользователям");
                }

                var userLocalRoles = users.Select(user => new UserCourseLocalRole()
                {
                    User = user,
                    Course = course,
                    LocalRole = role
                });

                //Add course and module completions for every invited user

                var localUsers = await users.ToArrayAsync();

                foreach (var localUser in localUsers)
                {
                    await completionInteractor.CreateCourseCompletion(localUser.Id, course.Id);

                    CourseModule[] courseModules = await unitOfWork.CourseModules.Where(courseModule => courseModule.CourseId == course.Id).ToArrayAsync();

                    foreach (var courseModule in courseModules)
                    {
                        await completionInteractor.CreateModuleCompletion(localUser.Id, courseModule.CourseId, courseModule.ModuleId);

                        var moduleLessons = await unitOfWork.ModuleLessons.Where(moduleLesson => moduleLesson.ModuleId == courseModule.ModuleId).ToArrayAsync();

                        foreach (var moduleLesson in moduleLessons)
                        {
                            await completionInteractor.CreateLessonCompletion(localUser.Id, moduleLesson.ModuleId, moduleLesson.LessonId);
                        }
                    }
                }


                await unitOfWork.Subscriptions.AddRangeAsync(subscriptions);
                await unitOfWork.UserCourseLocalRoles.AddRangeAsync(userLocalRoles);

                var count = users.Count();
                await unitOfWork.CommitAsync();

                await UpdateCourseSubscriptions(course.Id);

                return new Response()
                {
                    Success = true,
                    Message = $"Успешно записано {count} пользователей под ролью {role.Name}",
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
                await completionInteractor.CreateCourseCompletion(user.Id, course.Id);

                CourseModule[] courseModules = await unitOfWork.CourseModules.Where(courseModule => courseModule.CourseId == course.Id).ToArrayAsync();

                foreach (var courseModule in courseModules)
                {
                    await completionInteractor.CreateModuleCompletion(user.Id, courseModule.CourseId, courseModule.ModuleId);

                    var moduleLessons = await unitOfWork.ModuleLessons.Where(moduleLesson => moduleLesson.ModuleId == courseModule.ModuleId).ToArrayAsync();

                    foreach(var moduleLesson in moduleLessons)
                    {
                        await completionInteractor.CreateLessonCompletion(user.Id, moduleLesson.ModuleId, moduleLesson.LessonId);
                    }
                }

                await unitOfWork.CommitAsync();

                await UpdateCourseSubscriptions(course.Id);

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

                var courseUserLocalRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(userRole => userRole.UserId == userId && userRole.CourseId == courseId);

                if (courseUserLocalRole == null)
                {
                    throw new NotFoundException("Локальная роль пользователя не найдена");
                }

                if (subscription == null)
                {
                    throw new NotFoundException("Подписка не найдена");
                }

                unitOfWork.Subscriptions.Remove(subscription);
                unitOfWork.UserCourseLocalRoles.Remove(courseUserLocalRole);
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

                await unitOfWork.UserCourseLocalRoles.Entry(requesterCourseLocalRole)
                    .Reference(u => u.LocalRole)
                    .LoadAsync();

                unitOfWork.UserCourseLocalRoles.Entry(targetCourseLocalRole)
                    .Reference(u => u.LocalRole)
                    .Load();

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
                unitOfWork.UserCourseLocalRoles.Remove(targetCourseLocalRole);
                await unitOfWork.CommitAsync();
                await UpdateCourseSubscriptions(course.Id);

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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        private async Task UpdateCourseSubscriptions(int courseId)
        {
            var course = await unitOfWork.Courses.FindAsync(courseId);

            if(course == null)
            {
                return;
            }

            var subscriptionsCount = await unitOfWork.Subscriptions.Where(sub => sub.CourseId == courseId).CountAsync();

            course.SubscribersCount = subscriptionsCount;

            unitOfWork.Courses.Update(course);
            await unitOfWork.CommitAsync();
        }

    }
}
