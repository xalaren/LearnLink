using System.Linq.Expressions;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Repositories;

public interface IRoleRepository
{
    Task<bool> ExistsAsync(Expression<Func<Role, bool>> predicate);
    void Add(Role role);
}
