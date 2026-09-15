using RustyTail.Adapter.Configurations;
using RustyTail.Application.Abstractions.Data;
using RustyTail.Domain.Entities.Abstractions;
using RustyTail.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace RustyTail.Adapter.Contexts
{
    /// <summary>
    /// Entity Framework Core <see cref="DbContext"/> implementation for the
    /// adapter layer. Provides DbSet properties for domain entities and
    /// implements <see cref="IApplicationDataContext"/> to expose a
    /// commit operation used by the application.
    /// </summary>
    /// <param name="options">The options used by a <see cref="DbContext"/>.</param>
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IApplicationDataContext
    {
        /// <summary>
        /// Gets the users set.
        /// </summary>
        public DbSet<User> Users { get; init; }

        /// <summary>
        /// Gets the credentials set.
        /// </summary>
        public DbSet<Credentials> Credentials { get; init; }

        /// <summary>
        /// Gets the avatars set.
        /// </summary>
        public DbSet<Avatar> Avatars { get; init; }

        /// <summary>
        /// Gets the roles set.
        /// </summary>
        public DbSet<Role> Roles { get; init; }

        /// <summary>
        /// Gets the refresh tokens set.
        /// </summary>
        public DbSet<RefreshToken> RefreshTokens { get; init; }

        /// <summary>
        /// Persists changes to the database. This method updates auditable
        /// timestamps before calling <see cref="SaveChangesAsync"/>.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Configures the EF Core model by applying entity type configurations
        /// for the application's entities.
        /// </summary>
        /// <param name="modelBuilder">The model builder to configure.</param>
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
