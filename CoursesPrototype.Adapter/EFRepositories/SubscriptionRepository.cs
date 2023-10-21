using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Adapter.EFRepositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext context;

        public SubscriptionRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Subscription entity)
        {
            await context.Subscriptions.AddAsync(entity);
        }

        public async Task<Subscription[]> GetUserSubscriptions(int userId)
        {
            return await context.Subscriptions.Where(sub => sub.UserId == userId).ToArrayAsync();
        }

        public async Task RemoveAsync(int userId, int courseId)
        {
            var subscription = await context.Subscriptions.FirstOrDefaultAsync(subscription => subscription.UserId == userId && subscription.CourseId == courseId);

            if (subscription == null)
            {
                throw new NotFoundException("Подписка на курс не найдена");
            }

            context.Subscriptions.Remove(subscription);
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }
    }
}
