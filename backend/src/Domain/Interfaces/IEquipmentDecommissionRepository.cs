using Domain.Entities;
using Domain.Enumerations;

namespace Domain.Interfaces;

public interface IEquipmentDecommissionRepository : IRepository<EquipmentDecommission>
{
    /// <summary>
    /// Gets all decommissions for a specific equipment.
    /// </summary>
    Task<IEnumerable<EquipmentDecommission>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all decommissions performed by a specific technical.
    /// </summary>
    Task<IEnumerable<EquipmentDecommission>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all decommissions involving a specific department.
    /// </summary>
    Task<IEnumerable<EquipmentDecommission>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all decommissions by destiny type.
    /// </summary>
    Task<IEnumerable<EquipmentDecommission>> GetByDestinyTypeIdAsync(int destinyTypeId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all decommissions for a specific recipient.
    /// </summary>
    Task<IEnumerable<EquipmentDecommission>> GetByRecipientIdAsync(Guid recipientId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all decommissions within a date range.
    /// </summary>
    Task<IEnumerable<EquipmentDecommission>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all decommissions with pagination.
    /// </summary>
    Task<IEnumerable<EquipmentDecommission>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the latest decommission for a specific equipment.
    /// </summary>
    Task<EquipmentDecommission?> GetLatestByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if an equipment has any decommissions.
    /// </summary>
    Task<bool> HasDecommissionsAsync(Guid equipmentId, CancellationToken cancellationToken = default);
}