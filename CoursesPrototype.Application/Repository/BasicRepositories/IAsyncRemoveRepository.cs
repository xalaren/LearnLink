namespace CoursesPrototype.Application.Repository.BasicRepositories
{
    public interface IAsyncRemoveRepository
    {
        Task Remove(int entityId);
    }
}
