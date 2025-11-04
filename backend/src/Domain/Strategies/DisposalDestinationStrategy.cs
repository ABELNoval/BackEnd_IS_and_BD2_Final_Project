using Domain.Entities;

namespace Domain.Strategies;

/// <summary>
/// Strategy for handling equipment disposal.
/// When equipment is disposed, it moves to disposal location and becomes permanently decommissioned.
/// </summary>
public class DisposalDestinationStrategy : IDestinationStrategy
{
    public int DestinyTypeId => 2; // Disposal

    public Guid? TargetDepartmentId => null; // Disposal doesn't have a target department

    /// <summary>
    /// Applies disposal logic to the equipment.
    /// Equipment goes to disposal and department is cleared.
    /// </summary>
    public void ApplyTo(Equipment equipment)
    {
        equipment.MoveToDisposal();
    }
}
