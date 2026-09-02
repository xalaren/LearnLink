using System.Linq.Expressions;
using LearnLink.Adapter.Contexts;
using LearnLink.Application.Repositories;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Adapter.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public void Add(User user) => _context.Users.Add(user);

    public Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate) => _context.Users.AnyAsync(predicate);
}
