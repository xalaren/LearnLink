namespace CoursesPrototype.Application.Repository.BasicRepositories
{
    public interface IAsyncReadRepository<T> where T : class
    {
        Task<T?> Get(int entityId);
    }
}
