namespace CoursesPrototype.Application.RepositoryInterfaces
{
    public interface IAsyncRepository<T> where T: class
    {
        T[] GetAll();
        Task<T> Get(int entityId);
        Task Create(T entity);
        void Update(T entity);
        Task Remove(int entityId);
    }
}
