namespace CoursesPrototype.Application.Repository.BasicRepositories
{
    public interface IAsyncReadRepository<T> where T : class
    {
        Task<T?> GetAsync(int entityId);
    }
}
