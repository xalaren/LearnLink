namespace CoursesPrototype.Application.Transaction
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        void Commit();
    }
}
