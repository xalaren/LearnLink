using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Adapter.EFRepositories
{
    public class CredentialsRepository : ICredentialsRepository
    {
        private readonly AppDbContext context;

        public CredentialsRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Create(Credentials entity)
        {
            await context.AddAsync(entity);
        }

        public async Task<Credentials?> Get(int entityId)
        {
            return await context.Credentials.FindAsync(entityId);
        }

        public async Task<Credentials?> GetCredentialsByUserId(int userId)
        {
            return await context.Credentials.FirstOrDefaultAsync(credentials => credentials.UserId == userId);
        }

        public void Update(Credentials entity)
        {
            context.Credentials.Update(entity);
        }
    }
}
