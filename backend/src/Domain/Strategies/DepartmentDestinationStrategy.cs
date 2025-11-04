using Domain.Entities;

namespace Domain.Strategies;

/// <summary>
/// Strategy for handling equipment movement to a department.
/// When equipment is decommissioned to a department, it's assigned to that department.
/// </summary>
public class DepartmentDestinationStrategy : IDestinationStrategy
{
    private readonly Guid _targetDepartmentId;

    public int DestinyTypeId => 1; // Department

    public Guid? TargetDepartmentId => _targetDepartmentId;

    /// <summary>
    /// Creates a new department destination strategy
    /// </summary>
    /// <param name="targetDepartmentId">The ID of the department where the equipment will be moved</param>
    public DepartmentDestinationStrategy(Guid targetDepartmentId)
    {
        if (targetDepartmentId == Guid.Empty)
            throw new ArgumentException("Target department ID cannot be empty", nameof(targetDepartmentId));

        _targetDepartmentId = targetDepartmentId;
    }

    /// <summary>
    /// Applies department assignment logic to the equipment.
    /// Equipment is assigned to the target department.
    /// </summary>
    public void ApplyTo(Equipment equipment)
    {
        equipment.MoveToDepartment(_targetDepartmentId);
    }
}
