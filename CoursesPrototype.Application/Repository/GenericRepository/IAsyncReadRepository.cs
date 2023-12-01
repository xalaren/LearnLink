namespace CoursesPrototype.Application.Repository.GenericRepository
{
    public interface IAsyncReadRepository<T> where T : class
    {
        Task<T?> GetAsync(int entityId);
    }
}
