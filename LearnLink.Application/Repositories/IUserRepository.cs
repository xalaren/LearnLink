using System.Linq.Expressions;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);
        void Add(User user);
    }
}
