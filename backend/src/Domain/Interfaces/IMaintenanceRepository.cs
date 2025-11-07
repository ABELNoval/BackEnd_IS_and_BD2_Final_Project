using Domain.Entities;

namespace Domain.Interfaces;

public interface IMaintenanceRepository : IRepository<Maintenance>
{
    /// <summary>
    /// Gets all maintenance records for a specific equipment.
    /// </summary>
    Task<IEnumerable<Maintenance>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all maintenance records performed by a specific technical.
    /// </summary>
    Task<IEnumerable<Maintenance>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all maintenance records by maintenance type.
    /// </summary>
    Task<IEnumerable<Maintenance>> GetByMaintenanceTypeIdAsync(int maintenanceTypeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all maintenance records within a date range.
    /// </summary>
    Task<IEnumerable<Maintenance>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all maintenance records within a cost range.
    /// </summary>
    Task<IEnumerable<Maintenance>> GetByCostRangeAsync(decimal minCost, decimal maxCost, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all maintenance records with pagination.
    /// </summary>
    Task<IEnumerable<Maintenance>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest maintenance record for a specific equipment.
    /// </summary>
    Task<Maintenance?> GetLatestByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total maintenance cost for a specific equipment.
    /// </summary>
    Task<decimal> GetTotalCostByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total maintenance cost for a specific technical.
    /// </summary>
    Task<decimal> GetTotalCostByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of maintenance records by type.
    /// </summary>
    Task<int> CountByMaintenanceTypeIdAsync(int maintenanceTypeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all maintenance records for equipment in a specific date range by technical.
    /// </summary>
    Task<IEnumerable<Maintenance>> GetByTechnicalAndDateRangeAsync(Guid technicalId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}