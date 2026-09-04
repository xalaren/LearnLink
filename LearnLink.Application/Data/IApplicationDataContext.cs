using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Data;

public interface IApplicationDataContext
{
    public DbSet<User> Users { get; }
    public DbSet<Role> Roles { get; }
    public DbSet<Credentials> Credentials { get; } 
    public DbSet<RefreshToken> RefreshTokens { get; }
    Task CommitAsync(CancellationToken cancellationToken = default);
}
