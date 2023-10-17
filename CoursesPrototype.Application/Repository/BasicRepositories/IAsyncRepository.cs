namespace CoursesPrototype.Application.Repository.BasicRepositories
{
    public interface IAsyncRepository<T> : IAsyncReadRepository<T>, IAsyncWriteRepository<T>, IUpdateRepository<T>, IAsyncRemoveRepository where T : class
    {
    }
}
