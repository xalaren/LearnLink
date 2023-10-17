using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Adapter.EFRepositories
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : class
    {
        private readonly AppDbContext context;

        public AsyncRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Create(T entity)
        {
            await context.AddAsync(entity);
        }

        public Task<T> Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public T[] GetAll()
        {
            return context.Set<T>().ToArray();
        }

        public Task Remove(int entityId)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
