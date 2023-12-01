using System.Reflection;
using CoursesPrototype.Application.Mappers;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Core.Exceptions;
using CoursesPrototype.Shared.DataTransferObjects;
using CoursesPrototype.Shared.Responses;

namespace CoursesPrototype.Application.Interactors
{
    public class CourseInteractor
    {
        private readonly ICourseRepository courseRepository;
        private readonly IUserRepository userRepository;
        private readonly IUserCreatedCoursesRepository userCreatedCourseRepository;
        private readonly ISubscriptionRepository subscriptionRepository;
        private readonly IUnitOfWork unitOfWork;

        public CourseInteractor(ICourseRepository courseRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IUserCreatedCoursesRepository userCreatedCourseRepository, ISubscriptionRepository subscriptionRepository)
        {
            this.courseRepository = courseRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.userCreatedCourseRepository = userCreatedCourseRepository;
            this.subscriptionRepository = subscriptionRepository;
        }

        public async Task<Response<CourseDto?>> GetCourseAsync(int courseId)
        {
            try
            {
                var course = await courseRepository.GetAsync(courseId);

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

        public async Task<Response<CourseDto[]>> GetAllAsync()
        {
            try
            {

                var courses = await courseRepository.GetCourses();

                return new()
                {
                    Success = true,
                    Message = "Курсы успешно получены",
                    Value = courses.Select(course => course.ToDto()).ToArray(),
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
                var user = await userRepository.GetAsync(userId);

                if(user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                var userCreatedCourses = await userCreatedCourseRepository.GetUserCreatedCoursesAsync(userId);

                var courses = await courseRepository.GetByUserCreatedCoursesAsync(userCreatedCourses);

                return new()
                {
                    Success = true,
                    Message = "Курсы пользователя успешно получены",
                    Value = courses.Select(course => course.ToDto()).ToArray(),
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
                var user = await userRepository.GetAsync(userId);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                var subscriptions = await subscriptionRepository.GetUserSubscriptions(userId);

                var courses = await courseRepository.GetSubscribedCourses(subscriptions);

                return new()
                {
                    Success = true,
                    Message = "Курсы пользователя успешно получены",
                    Value = courses.Select(course => course.ToDto()).ToArray(),
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

                var user = await userRepository.GetAsync(userId);

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

                await userCreatedCourseRepository.CreateAsync(userCreatedCourse);

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
                var courses = await courseRepository.GetPublicAsync();

                return new()
                {
                    Success = true,
                    Message = "Курсы успешно получены",
                    Value = courses.Select(course => course.ToDto()).ToArray(),
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
                
                var course = await courseRepository.GetAsync(courseDto.Id);

                if(course == null)
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
                await courseRepository.RemoveAsync(courseId);
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
    }
}
