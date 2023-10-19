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
        private readonly IUnitOfWork unitOfWork;

        public CourseInteractor(ICourseRepository courseRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IUserCreatedCoursesRepository userCreatedCourseRepository)
        {
            this.courseRepository = courseRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.userCreatedCourseRepository = userCreatedCourseRepository;
        }

        public async Task<Response<CourseDto[]>> GetUserCreatedCoursesAsync(int userId)
        {
            try
            {
                var user = await userRepository.GetAsync(userId);

                if(user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                var courses = await userCreatedCourseRepository.GetUserCreatedCoursesAsync(userId);

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

                await userCreatedCourseRepository.Create(userCreatedCourse);

                unitOfWork.Commit();

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
                var course = await courseRepository.GetAsync(courseDto.Id);

                if(course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                course = course.Assign(courseDto);

                unitOfWork.Commit();

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
                unitOfWork.Commit();

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
