using Domain.Entities;

namespace Domain.Interfaces;

public interface IResponsibleRepository : IRepository<Responsible>
{
    /// <summary>
    /// Gets a responsible by name (case-insensitive).
    /// </summary>
    Task<Responsible?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a responsible by email.
    /// </summary>
    Task<Responsible?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all responsibles with pagination.
    /// </summary>
    Task<IEnumerable<Responsible>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a responsible with the given email exists.
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the responsible for a specific department.
    /// </summary>
    Task<Responsible?> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a responsible is managing any department.
    /// </summary>
    Task<bool> IsManagingDepartmentAsync(Guid responsibleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all responsibles who authorized transfers.
    /// </summary>
    Task<IEnumerable<Responsible>> GetResponsiblesWithTransfersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets count of transfers authorized by a responsible.
    /// </summary>
    Task<int> GetTransferCountAsync(Guid responsibleId, CancellationToken cancellationToken = default);
}