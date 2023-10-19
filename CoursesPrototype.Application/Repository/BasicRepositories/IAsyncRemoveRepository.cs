namespace CoursesPrototype.Application.Repository.BasicRepositories
{
    public interface IAsyncRemoveRepository
    {
        Task RemoveAsync(int entityId);
    }
}
