using LearnLink.Adapter.Configurations;
using LearnLink.Application.Repositories;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Adapter.Contexts
{
    public class AppDbContext(DbContextOptions options) : DbContext(options), IAppRepository
    {
        public DbSet<User> Users { get; init; }
        public DbSet<Credentials> Credentials { get; init; }
        public DbSet<Avatar> Avatars { get; init; }
        public DbSet<Role> Roles { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AvatarsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CredentialsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        }
    }
}
