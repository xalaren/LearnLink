namespace CoursesPrototype.Application.Repository.GenericRepository
{
    public interface IAsyncWriteRepository<T> where T : class
    {
        Task CreateAsync(T entity);
    }
}
