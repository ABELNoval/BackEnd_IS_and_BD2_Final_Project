using Domain.Common;

namespace Domain.Enumerations;

/// <summary>
/// Enumeration representing the status of a maintenance operation.
/// </summary>
public class MaintenanceStatus : Enumeration
{
    /// <summary>
    /// Maintenance is in progress
    /// </summary>
    public static readonly MaintenanceStatus InProgress = new(1, "InProgress");

    /// <summary>
    /// Maintenance has been completed
    /// </summary>
    public static readonly MaintenanceStatus Completed = new(2, "Completed");

    private MaintenanceStatus(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Gets a MaintenanceStatus by its ID
    /// </summary>
    public static MaintenanceStatus FromId(int id)
    {
        return FromValue<MaintenanceStatus>(id);
    }

    /// <summary>
    /// Gets all available maintenance statuses
    /// </summary>
    public static IEnumerable<MaintenanceStatus> GetAll()
    {
        yield return InProgress;
        yield return Completed;
    }
}
