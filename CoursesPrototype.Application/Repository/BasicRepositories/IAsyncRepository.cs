namespace CoursesPrototype.Application.Repository.BasicRepositories
{
    public interface IAsyncRepository<T> : IAsyncReadRepository<T>, IAsyncWriteRepository<T>, IUpdateRepository<T>, IAsyncRemoveRepository, IAsyncDisposable where T : class
    {
    }
}
