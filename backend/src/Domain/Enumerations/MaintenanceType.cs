using Domain.Common;

namespace Domain.Enumerations;

/// <summary>
/// Enumeration representing the types of maintenance that can be performed on equipment.
/// Uses the Enumeration pattern to provide type-safe, behavior-rich constants.
/// </summary>
public class MaintenanceType : Enumeration
{
    /// <summary>
    /// Preventive maintenance - scheduled regular maintenance
    /// </summary>
    public static readonly MaintenanceType Preventive = new(1, "Preventivo");

    /// <summary>
    /// Corrective maintenance - fixing failures or issues
    /// </summary>
    public static readonly MaintenanceType Corrective = new(2, "Correctivo");

    /// <summary>
    /// Predictive maintenance - based on condition monitoring
    /// </summary>
    public static readonly MaintenanceType Predictive = new(3, "Predictivo");

    /// <summary>
    /// Emergency maintenance - urgent repairs
    /// </summary>
    public static readonly MaintenanceType Emergency = new(4, "Emergencia");

    private MaintenanceType(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Indicates if this maintenance type is urgent
    /// </summary>
    public bool IsUrgent => Id == Emergency.Id || Id == Corrective.Id;

    /// <summary>
    /// Indicates if this maintenance type is planned
    /// </summary>
    public bool IsPlanned => Id == Preventive.Id || Id == Predictive.Id;

    /// <summary>
    /// Gets all available maintenance types
    /// </summary>
    public static IEnumerable<MaintenanceType> GetAll()
    {
        yield return Preventive;
        yield return Corrective;
        yield return Predictive;
        yield return Emergency;
    }

    /// <summary>
    /// Gets a maintenance type by ID
    /// </summary>
    public static MaintenanceType? FromId(int id)
    {
        return GetAll().FirstOrDefault(m => m.Id == id);
    }

    /// <summary>
    /// Gets a maintenance type by name
    /// </summary>
    public static MaintenanceType? FromName(string name)
    {
        return GetAll().FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
