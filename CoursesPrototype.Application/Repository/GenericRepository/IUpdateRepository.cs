namespace CoursesPrototype.Application.Repository.GenericRepository
{
    public interface IUpdateRepository<T> where T : class
    {
        void Update(T entity);
    }
}
