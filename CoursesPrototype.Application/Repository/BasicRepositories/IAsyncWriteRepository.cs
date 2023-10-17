namespace CoursesPrototype.Application.Repository.BasicRepositories
{
    public interface IAsyncWriteRepository<T> where T : class
    {
        Task Create(T entity);
        void Update(T entity);
    }
}
