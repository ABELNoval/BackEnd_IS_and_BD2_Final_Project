using Domain.Entities;

namespace Domain.Interfaces;

public interface IRoleRepository : IRepository<Role>
{
    /// <summary>
    /// Gets a role by ID.
    /// </summary>
    Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a role by name (case-insensitive).
    /// </summary>
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all available roles.
    /// </summary>
    Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of users with a specific role.
    /// </summary>
    Task<int> GetUserCountByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a role with the given ID exists.
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}