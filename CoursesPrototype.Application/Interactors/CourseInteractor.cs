using CoursesPrototype.Application.Mappers;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Application.Repository.BasicRepositories;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Shared.Exceptions;
using CoursesPrototype.Shared.ToClientData.DataTransferObjects;
using CoursesPrototype.Shared.ToClientData.Responses;

namespace CoursesPrototype.Application.Interactors
{
    public class CourseInteractor
    {
        private readonly ICourseRepository courseRepository;
        private readonly IUserRepository userRepository;
        private readonly IAsyncRepository<UserCreatedCourse> userCreatedCourseRepository;
        private readonly IUnitOfWork unitOfWork;

        public CourseInteractor(ICourseRepository courseRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IAsyncRepository<UserCreatedCourse> userCreatedCourseRepository)
        {
            this.courseRepository = courseRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.userCreatedCourseRepository = userCreatedCourseRepository;
        }

        public async Task<Response> CreateCourseAsync(int userId, CourseDto courseDto)
        {
            try
            {
                if(courseDto == null)
                {
                    throw new ArgumentNullException("CourseDto was null");
                }

                var user = await userRepository.Get(userId);

                if(user == null)
                {
                    throw new ArgumentNullException("User not found");
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
            catch(ForClientSideBaseException exception) 
            {
                return new Response()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
            catch(Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = "Не удалось создать курс. Внутренняя ошибка",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }
    }
}
