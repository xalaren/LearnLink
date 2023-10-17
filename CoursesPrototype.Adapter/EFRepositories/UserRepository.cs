using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Adapter.EFRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Create(User entity)
        {
            await context.Users.AddAsync(entity);
        }

        public async Task<User?> Get(int entityId)
        {
            return await context.Users.FindAsync(entityId);
        }

        public async Task<User[]> GetAll()
        {
            return await context.Users.ToArrayAsync();
        }

        public async Task<User?> GetByNickname(string nickname)
        {
            return await context.Users.FirstOrDefaultAsync(user => user.Nickname == nickname);
        }

        public Task Remove(int entityId)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
