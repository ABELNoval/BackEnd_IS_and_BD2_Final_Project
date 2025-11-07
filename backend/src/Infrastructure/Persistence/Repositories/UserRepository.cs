using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.Value.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.RoleId == roleId)
            .OrderBy(u => u.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .OrderBy(u => u.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.Value.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<int> CountByRoleIdAsync(int roleId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .CountAsync(u => u.RoleId == roleId, cancellationToken);
    }

    public async Task<IEnumerable<User>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Name.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(u => u.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .OrderBy(u => u.Name)
            .ToListAsync(cancellationToken);
    }
}