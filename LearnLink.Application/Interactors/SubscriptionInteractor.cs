using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
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

        public async Task<Response> CreateSubscriptionAsync(SubscriptionDto subscriptionDto)
        {
            try
            {
                if(subscriptionDto == null)
                {
                    throw new ArgumentNullException(nameof(subscriptionDto), "SubscriptionDto was null");
                }

                var user = await unitOfWork.Users.FindAsync(subscriptionDto.UserId);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                var course = await unitOfWork.Courses.FindAsync(subscriptionDto.CourseId);

                if (course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                var userCreatedCourse = await unitOfWork.UserCreatedCourses.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == user.Id && u.CourseId == course.Id);

                if(userCreatedCourse != null)
                {
                    throw new AccessLevelException("Пользователь является создателем этого курса");
                }

                //TODO: possibly it is necessary to lock the subscription for private courses

                var subscriptionEntity = subscriptionDto.ToEntity();

                subscriptionEntity.User = user;
                subscriptionEntity.Course = course;


                await unitOfWork.Subscriptions.AddAsync(subscriptionEntity);
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
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> Unsubscribe(int userId, int courseId)
        {
            try
            {
                var subscription = await unitOfWork.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.CourseId == courseId);

                if(subscription == null)
                {
                    throw new NotFoundException("Подписка не найдена");
                }

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
    }
}
