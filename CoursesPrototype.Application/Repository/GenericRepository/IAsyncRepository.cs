namespace CoursesPrototype.Application.Repository.GenericRepository
{
    public interface IAsyncRepository<T> : IAsyncReadRepository<T>, IAsyncWriteRepository<T>, IUpdateRepository<T>, IAsyncRemoveRepository, IAsyncDisposable where T : class
    {
    }
}
