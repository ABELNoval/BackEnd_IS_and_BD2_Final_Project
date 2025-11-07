using Domain.Entities;

namespace Domain.Interfaces;

public interface ITransferRepository : IRepository<Transfer>
{
    /// <summary>
    /// Gets all transfers for a specific equipment.
    /// </summary>
    Task<IEnumerable<Transfer>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all transfers from a specific source department.
    /// </summary>
    Task<IEnumerable<Transfer>> GetBySourceDepartmentIdAsync(Guid sourceDepartmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all transfers to a specific target department.
    /// </summary>
    Task<IEnumerable<Transfer>> GetByTargetDepartmentIdAsync(Guid targetDepartmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all transfers involving a specific department (source or target).
    /// </summary>
    Task<IEnumerable<Transfer>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all transfers authorized by a specific responsible.
    /// </summary>
    Task<IEnumerable<Transfer>> GetByResponsibleIdAsync(Guid responsibleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all transfers within a date range.
    /// </summary>
    Task<IEnumerable<Transfer>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all transfers with pagination.
    /// </summary>
    Task<IEnumerable<Transfer>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest transfer for a specific equipment.
    /// </summary>
    Task<Transfer?> GetLatestByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets transfers between two specific departments.
    /// </summary>
    Task<IEnumerable<Transfer>> GetByDepartmentPairAsync(Guid sourceDepartmentId, Guid targetDepartmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets count of transfers for a specific equipment.
    /// </summary>
    Task<int> CountByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets count of transfers involving a department.
    /// </summary>
    Task<int> CountByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the most recent transfers across all equipment.
    /// </summary>
    Task<IEnumerable<Transfer>> GetRecentTransfersAsync(int count, CancellationToken cancellationToken = default);
}