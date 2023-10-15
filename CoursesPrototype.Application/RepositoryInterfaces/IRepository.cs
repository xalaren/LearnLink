namespace CoursesPrototype.Application.RepositoryInterfaces
{
    public interface IRepository<T> where T: class
    {
        void Get(int entityId);
        void Create(T entity);
        void Update(T entity);
        void Remove(int entityId);
    }
}
