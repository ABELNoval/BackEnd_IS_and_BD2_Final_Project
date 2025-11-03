namespace Domain.Strategies;

/// <summary>
/// Strategy interface for handling equipment destination logic.
/// Each destination type (disposal, warehouse, department) has its own implementation.
/// </summary>
public interface IDestinationStrategy
{
    /// <summary>
    /// ID of the destiny type (from DestinyType enumeration)
    /// </summary>
    int DestinyTypeId { get; }

    /// <summary>
    /// Target department ID (only for Department destination, null for others)
    /// </summary>
    Guid? TargetDepartmentId { get; }

    /// <summary>
    /// Applies the destination logic to the equipment.
    /// This method modifies the equipment's internal state based on the destination type.
    /// </summary>
    /// <param name="equipment">The equipment to apply the destination to</param>
    void ApplyTo(Equipment equipment);
}
