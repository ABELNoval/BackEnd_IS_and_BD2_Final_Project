using Domain.Common;
using Domain.Strategies;

namespace Domain.Enumerations;

/// <summary>
/// Enumeration representing the possible destiny types for equipment decommission.
/// Uses the Enumeration pattern to provide type-safe, behavior-rich constants.
/// </summary>
public class DestinyType : Enumeration
{
    /// <summary>
    /// Equipment is moved to a specific department
    /// </summary>
    public static readonly DestinyType Department = new(1, "departamento");

    /// <summary>
    /// Equipment is disposed/scrapped
    /// </summary>
    public static readonly DestinyType Disposal = new(2, "desecho");

    /// <summary>
    /// Equipment is moved to warehouse/storage
    /// </summary>
    public static readonly DestinyType Warehouse = new(3, "almacén");

    private DestinyType(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Indicates if this destiny type requires a target department ID
    /// </summary>
    public bool RequiresDepartment => Id == Department.Id;

    /// <summary>
    /// Creates the appropriate destination strategy for this destiny type
    /// </summary>
    /// <param name="targetDepartmentId">The target department ID (required only for Department destiny)</param>
    /// <returns>The destination strategy implementation</returns>
    public IDestinationStrategy CreateStrategy(Guid? targetDepartmentId = null)
    {
        if (Id == Department.Id)
        {
            if (!targetDepartmentId.HasValue || targetDepartmentId.Value == Guid.Empty)
                throw new ArgumentNullException(nameof(targetDepartmentId), "Department destiny requires a target department ID");
            return new DepartmentDestinationStrategy(targetDepartmentId.Value);
        }

        if (Id == Disposal.Id)
            return new DisposalDestinationStrategy();

        if (Id == Warehouse.Id)
            return new WarehouseDestinationStrategy();

        throw new InvalidOperationException($"Unknown destiny type: {Id}");
    }

    /// <summary>
    /// Gets all available destiny types
    /// </summary>
    public static IEnumerable<DestinyType> GetAll()
    {
        yield return Department;
        yield return Disposal;
        yield return Warehouse;
    }

    /// <summary>
    /// Gets a destiny type by ID
    /// </summary>
    public static DestinyType FromId(int id)
    {
        return GetAll().FirstOrDefault(d => d.Id == id)
            ?? throw new ArgumentException($"Invalid destiny type ID: {id}", nameof(id));
    }

    /// <summary>
    /// Gets a destiny type by name
    /// </summary>
    public static DestinyType FromName(string name)
    {
        return GetAll().FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"Invalid destiny type name: {name}", nameof(name));
    }
}
