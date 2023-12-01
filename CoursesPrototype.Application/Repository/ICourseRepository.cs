using CoursesPrototype.Application.Repository.GenericRepository;
using CoursesPrototype.Core.Entities;

namespace CoursesPrototype.Application.Repository
{
    public interface ICourseRepository : IAsyncRepository<Course>, IAsyncDisposable
    {
        Task<Course[]> GetCourses();
        Task<Course[]> GetPublicAsync();
        Task<Course[]> GetByUserCreatedCoursesAsync(IEnumerable<UserCreatedCourse> userCreatedCourses);
        Task<Course[]> GetSubscribedCourses(IEnumerable<Subscription> subscriptions);
    }
}
