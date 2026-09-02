using LearnLink.Adapter.Contexts;
using LearnLink.Application.Repositories;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Adapter.Repositories;

public class CredentialsRepository(AppDbContext context) : ICredentialsRepository
{
    private readonly AppDbContext _context = context;

    public void Add(Credentials credentials)
    {
        _context.Credentials.Add(credentials);
    }
}
