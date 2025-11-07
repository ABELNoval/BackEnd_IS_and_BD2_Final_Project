using Domain.Entities;

namespace Domain.Interfaces;

public interface IEquipmentTypeRepository : IRepository<EquipmentType>
{
    /// <summary>
    /// Gets an equipment type by name (case-insensitive).
    /// </summary>
    Task<EquipmentType?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all equipment types with pagination.
    /// </summary>
    Task<IEnumerable<EquipmentType>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an equipment type with the given name exists.
    /// </summary>
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets equipment types whose names contain the search term (case-insensitive).
    /// </summary>
    Task<IEnumerable<EquipmentType>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of equipment associated with this type.
    /// </summary>
    Task<int> GetEquipmentCountByTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all equipment types ordered by name.
    /// </summary>
    Task<IEnumerable<EquipmentType>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default);
}