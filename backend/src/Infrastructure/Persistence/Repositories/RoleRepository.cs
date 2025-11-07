using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }

    public Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        // Role is an Enumeration, so we use the static methods instead of DB queries
        var role = Role.FromId(id);
        return Task.FromResult(role);
    }

    public Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        // Role is an Enumeration, so we use the static methods instead of DB queries
        var role = Role.FromName(name);
        return Task.FromResult(role);
    }

    public Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        // Role is an Enumeration, so we use the static GetAll() method
        var roles = Role.GetAll();
        return Task.FromResult(roles);
    }

    public async Task<int> GetUserCountByRoleIdAsync(int roleId, CancellationToken cancellationToken = default)
    {
        // Users table contains all user types with RoleId
        return await _context.Users
            .CountAsync(u => u.RoleId == roleId, cancellationToken);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        // Role is an Enumeration, check if the role exists in the static list
        var exists = Role.FromId(id) != null;
        return Task.FromResult(exists);
    }
}