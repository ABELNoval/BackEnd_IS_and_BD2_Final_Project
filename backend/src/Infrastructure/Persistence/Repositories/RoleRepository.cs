using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }

    public new async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // DbSet para obtener las entidades de la base de datos
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public new async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // DbSet para buscar por ID
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public new async Task AddAsync(Role entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public new async Task UpdateAsync(Role entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public new async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var role = Role.FromId(id);
        return Task.FromResult(role);
    }

    public Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        // Role es una Enumeration, así que usamos los métodos estáticos en lugar de consultas DB
        var role = Role.FromName(name);
        return Task.FromResult(role);
    }

    public Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = Role.GetAll();
        return Task.FromResult(roles);
    }

    public async Task<int> GetUserCountByRoleIdAsync(int roleId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .CountAsync(u => u.RoleId == roleId, cancellationToken);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = Role.FromId(id) != null;
        return Task.FromResult(exists);
    }
}