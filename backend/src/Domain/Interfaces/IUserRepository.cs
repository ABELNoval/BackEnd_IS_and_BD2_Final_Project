using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Gets a user by email (across all user types).
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by name (case-insensitive, across all user types).
    /// </summary>
    Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all users by role ID.
    /// </summary>
    Task<IEnumerable<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all users with pagination.
    /// </summary>
    Task<IEnumerable<User>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user with the given email exists.
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets count of users by role.
    /// </summary>
    Task<int> CountByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches users by name (case-insensitive, partial match).
    /// </summary>
    Task<IEnumerable<User>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all users ordered by name.
    /// </summary>
    Task<IEnumerable<User>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default);
}