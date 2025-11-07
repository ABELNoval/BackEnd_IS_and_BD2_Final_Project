using Domain.Entities;

namespace Domain.Interfaces;

public interface IEquipmentRepository : IRepository<Equipment>
{
    /// <summary>
    /// Gets equipment by name (case-insensitive).
    /// </summary>
    Task<Equipment?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all equipment of a specific type.
    /// </summary>
    Task<IEnumerable<Equipment>> GetByEquipmentTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all equipment in a specific department.
    /// </summary>
    Task<IEnumerable<Equipment>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all equipment by state.
    /// </summary>
    Task<IEnumerable<Equipment>> GetByStateAsync(int stateId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all equipment by location type.
    /// </summary>
    Task<IEnumerable<Equipment>> GetByLocationTypeAsync(int locationTypeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all equipment acquired within a date range.
    /// </summary>
    Task<IEnumerable<Equipment>> GetByAcquisitionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all equipment with pagination.
    /// </summary>
    Task<IEnumerable<Equipment>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets equipment with all related data (decommissions, transfers, maintenances).
    /// </summary>
    Task<Equipment?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all operative equipment in a department.
    /// </summary>
    Task<IEnumerable<Equipment>> GetOperativeByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all equipment under maintenance.
    /// </summary>
    Task<IEnumerable<Equipment>> GetEquipmentUnderMaintenanceAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all equipment in warehouse.
    /// </summary>
    Task<IEnumerable<Equipment>> GetWarehouseEquipmentAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if equipment with the given name exists.
    /// </summary>
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets count of equipment by state.
    /// </summary>
    Task<int> CountByStateAsync(int stateId, CancellationToken cancellationToken = default);
}