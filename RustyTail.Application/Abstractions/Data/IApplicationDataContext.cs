using RustyTail.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace RustyTail.Application.Abstractions.Data;

/// <summary>
/// Abstraction of the application's data context used by the application
/// layer. Exposes entity sets required by the domain and a commit
/// operation for persisting changes.
/// </summary>
public interface IApplicationDataContext
{
    /// <summary>
    /// Gets the <see cref="DbSet{User}"/> representing users.
    /// </summary>
    public DbSet<User> Users { get; }

    /// <summary>
    /// Gets the <see cref="DbSet{Role}"/> representing roles.
    /// </summary>
    public DbSet<Role> Roles { get; }

    /// <summary>
    /// Gets the <see cref="DbSet{Credentials}"/> representing user
    /// credentials.
    /// </summary>
    public DbSet<Credentials> Credentials { get; }

    /// <summary>
    /// Gets the <see cref="DbSet{RefreshToken}"/> representing refresh
    /// tokens used for authentication flows.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; }

    /// <summary>
    /// Persists changes in the data context to the underlying store.
    /// Implementations should apply any required pre-save logic (for
    /// example updating audit fields) before saving.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting
    /// for the operation to complete.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);
}
