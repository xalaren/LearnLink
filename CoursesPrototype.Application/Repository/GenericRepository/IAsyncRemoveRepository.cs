namespace CoursesPrototype.Application.Repository.GenericRepository
{
    public interface IAsyncRemoveRepository
    {
        Task RemoveAsync(int entityId);
    }
}
