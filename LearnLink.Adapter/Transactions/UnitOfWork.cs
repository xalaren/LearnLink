using LearnLink.Adapter.Contexts;
using LearnLink.Application.Repositories;
using LearnLink.Application.Transactions;
using LearnLink.Core.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Adapter.Transactions
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;

        public IAppRepository Repository => _context;

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return _context.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var utcNow = DateTime.UtcNow;

            var entries = _context.ChangeTracker.Entries<IAuditable>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetCreated(utcNow);
                    entry.Entity.SetModified(utcNow);
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.SetModified(utcNow);
                }
            }
        }
    }
}
