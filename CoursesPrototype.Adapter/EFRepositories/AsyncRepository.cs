using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Repository.BasicRepositories;

namespace CoursesPrototype.Adapter.EFRepositories
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : class
    {
        private readonly AppDbContext context;

        public AsyncRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(T entity)
        {
            await context.AddAsync(entity);
        }

        public async Task<T?> GetAsync(int entityId)
        {
            return await context.FindAsync<T>(entityId);
        }

        public async Task RemoveAsync(int entityId)
        {
            var entity = await GetAsync(entityId);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity not found");
            }

            context.Remove(entity);
        }

        public void Update(T entity)
        {
            context.Update(entity);
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }
    }
}
