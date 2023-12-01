using CoursesPrototype.Application.Mappers;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Exceptions;
using CoursesPrototype.Shared.DataTransferObjects;
using CoursesPrototype.Shared.Responses;

namespace CoursesPrototype.Application.Interactors
{
    public class SubscriptionInteractor
    {
        private readonly ICourseRepository courseRepository;
        private readonly IUserRepository userRepository;
        private readonly ISubscriptionRepository subscriptionRepository;
        private readonly IUserCreatedCoursesRepository userCreatedCoursesRepository;

        private readonly IUnitOfWork unitOfWork;

        public SubscriptionInteractor(
            ICourseRepository courseRepository,
            IUserRepository userRepository,
            ISubscriptionRepository subscriptionRepository,
            IUserCreatedCoursesRepository userCreatedCoursesRepository,
            IUnitOfWork unitOfWork)
        {
            this.courseRepository = courseRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.subscriptionRepository = subscriptionRepository;
            this.userCreatedCoursesRepository = userCreatedCoursesRepository;
        }

        public async Task<Response> CreateSubscriptionAsync(SubscriptionDto subscriptionDto)
        {
            try
            {
                if(subscriptionDto == null)
                {
                    throw new ArgumentNullException(nameof(subscriptionDto), "SubscriptionDto was null");
                }

                var user = await userRepository.GetAsync(subscriptionDto.UserId);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                var course = await courseRepository.GetAsync(subscriptionDto.CourseId);

                if (course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                var userCreatedCourse = await userCreatedCoursesRepository.GetUserCreatedCourse(user.Id, course.Id);

                if(userCreatedCourse != null)
                {
                    throw new AccessLevelException("Пользователь является создателем этого курса");
                }

                //TODO: possibly it is necessary to lock the subscription for private courses

                var subscriptionEntity = subscriptionDto.ToEntity();

                subscriptionEntity.User = user;
                subscriptionEntity.Course = course;


                await subscriptionRepository.CreateAsync(subscriptionEntity);
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
                await subscriptionRepository.RemoveAsync(userId, courseId);
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
