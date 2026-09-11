using LearnLink.Adapter.Configurations;
using LearnLink.Application.Abstractions.Data;
using LearnLink.Domain.Entities.Abstractions;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Adapter.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IApplicationDataContext
    {
        public DbSet<User> Users { get; init; }
        public DbSet<Credentials> Credentials { get; init; }
        public DbSet<Avatar> Avatars { get; init; }
        public DbSet<Role> Roles { get; init; }
        public DbSet<RefreshToken> RefreshTokens { get; init; }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AvatarsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CredentialsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshToeknEntityTypeConfiguration());
        }

        private void UpdateTimestamps()
        {
            var utcNow = DateTime.UtcNow;

            var entries = ChangeTracker.Entries<IAuditable>();
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
