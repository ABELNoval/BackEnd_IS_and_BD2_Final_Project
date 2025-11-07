using Domain.Entities;

namespace Domain.Interfaces;

public interface ISectionRepository : IRepository<Section>
{
    /// <summary>
    /// Gets a section by name (case-insensitive).
    /// </summary>
    Task<Section?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all sections with pagination.
    /// </summary>
    Task<IEnumerable<Section>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a section with the given name exists.
    /// </summary>
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets sections whose names contain the search term (case-insensitive).
    /// </summary>
    Task<IEnumerable<Section>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of departments in a section.
    /// </summary>
    Task<int> GetDepartmentCountBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all sections ordered by name.
    /// </summary>
    Task<IEnumerable<Section>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a section has any departments.
    /// </summary>
    Task<bool> HasDepartmentsAsync(Guid sectionId, CancellationToken cancellationToken = default);
}