namespace CoursesPrototype.Application.Repository.BasicRepositories
{
    public interface IUpdateRepository<T> where T : class
    {
        void Update(T entity);
    }
}
