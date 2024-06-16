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
    public class SubscriptionInteractor(
        IUnitOfWork unitOfWork,
        CompletionInteractor completionInteractor,
        UserCourseLocalRolesInteractor userCourseLocalRolesInteractor)
    {
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

        public async Task<Response> InviteAsync(int userId, int courseId, int localRoleId, params int[] userIdentifiers)
        {
            try
            {
                var course = await unitOfWork.Courses.FindAsync(courseId) ??
                    throw new NotFoundException("Курс не найден");

                var user = await unitOfWork.Users.FindAsync(userId) ?? throw new NotFoundException("Пользователь не найден");

                var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles
                    .Include(userCourseLocalRole => userCourseLocalRole.LocalRole)
                    .FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId) ??
                        throw new NotFoundException("Не удалось определить локальную роль пользователя");


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

                var count = users.Count();
                await unitOfWork.CommitAsync();

                await UpdateCourseSubscriptions(course.Id);
                await userCourseLocalRolesInteractor.RequestCreateAsyncNoResponse(userId, courseId, localRoleId, localUsers);

                return new Response()
                {
                    Success = true,
                    Message = $"Успешно записано {count} пользователей",
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

                var role = await unitOfWork.LocalRoles.FirstOrDefaultAsync(role => string.Equals(role.Sign, RoleDataConstants.MEMBER_ROLE_SIGN));

                if (role == null)
                {
                    throw new NotFoundException("Локальная роль получена неверно, либо не найдена");
                }

                var subscriptionEntity = subscriptionDto.ToEntity();

                subscriptionEntity.User = user;
                subscriptionEntity.Course = course;

                await unitOfWork.Subscriptions.AddAsync(subscriptionEntity);
                await completionInteractor.CreateCourseCompletion(user.Id, course.Id);

                CourseModule[] courseModules = await unitOfWork.CourseModules.Where(courseModule => courseModule.CourseId == course.Id).ToArrayAsync();

                foreach (var courseModule in courseModules)
                {
                    await completionInteractor.CreateModuleCompletion(user.Id, courseModule.CourseId, courseModule.ModuleId);

                    var moduleLessons = await unitOfWork.ModuleLessons.Where(moduleLesson => moduleLesson.ModuleId == courseModule.ModuleId).ToArrayAsync();

                    foreach (var moduleLesson in moduleLessons)
                    {
                        await completionInteractor.CreateLessonCompletion(user.Id, moduleLesson.ModuleId, moduleLesson.LessonId);
                    }
                }

                await unitOfWork.CommitAsync();
                await UpdateCourseSubscriptions(course.Id);
                await AssignMembersAsync(user.Id, course.Id);

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

                if (subscription == null)
                {
                    throw new NotFoundException("Подписка не найдена");
                }

                unitOfWork.Subscriptions.Remove(subscription);
                await unitOfWork.CommitAsync();

                await UpdateCourseSubscriptions(courseId);
                await userCourseLocalRolesInteractor.RemoveAsyncNoResponse(userId, courseId);

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
                if (requesterUserId == targetUserId)
                {
                    throw new AccessLevelException("Невозможно исключить себя");
                }

                Subscription subscription = await unitOfWork.Subscriptions.FirstOrDefaultAsync(s => s.UserId == targetUserId && s.CourseId == courseId) ??
                    throw new NotFoundException("Подписка не найдена");

                unitOfWork.Subscriptions.Remove(subscription);
                await unitOfWork.CommitAsync();
                await UpdateCourseSubscriptions(courseId);
                await userCourseLocalRolesInteractor.RequestRemoveAsyncNoResponse(requesterUserId, targetUserId, courseId);

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
                    Message = "Не удалось исключить пользователей от курса",
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        private async Task UpdateCourseSubscriptions(int courseId)
        {
            var course = await unitOfWork.Courses.FindAsync(courseId);

            if (course == null)
            {
                return;
            }

            var subscriptionsCount = await unitOfWork.Subscriptions.Where(sub => sub.CourseId == courseId).CountAsync();

            course.SubscribersCount = subscriptionsCount;

            unitOfWork.Courses.Update(course);
            await unitOfWork.CommitAsync();
        }
        
        private async Task AssignMembersAsync(int userId, int courseId)
        {
            var memberRole = await unitOfWork.LocalRoles.FirstOrDefaultAsync(localRole => localRole.Sign == RoleDataConstants.MEMBER_ROLE_SIGN);

            if(memberRole == null)
            {
                throw new NotFoundException("Роль участника не найдена");
            }

            await userCourseLocalRolesInteractor.CreateAsyncNoResponse(userId, courseId, memberRole.Id);
        }
    }
}
