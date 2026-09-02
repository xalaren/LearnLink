using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LearnLink.Adapter.Contexts;
using LearnLink.Application.Repositories;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Adapter.Repositories
{
    public class RoleRepository(AppDbContext context) : IRoleRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(Role role) => _context.Roles.Add(role);
        public Task<bool> ExistsAsync(Expression<Func<Role, bool>> predicate) => _context.Roles.AnyAsync(predicate);
    }
}
