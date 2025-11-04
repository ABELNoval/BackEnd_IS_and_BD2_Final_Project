using Domain.Entities;

namespace Domain.Strategies;

/// <summary>
/// Strategy for handling equipment movement to warehouse.
/// When equipment goes to warehouse, it's stored and department is cleared.
/// </summary>
public class WarehouseDestinationStrategy : IDestinationStrategy
{
    public int DestinyTypeId => 3; // Warehouse

    public Guid? TargetDepartmentId => null; // Warehouse doesn't have a target department

    /// <summary>
    /// Applies warehouse logic to the equipment.
    /// Equipment goes to warehouse and department is cleared.
    /// </summary>
    public void ApplyTo(Equipment equipment)
    {
        equipment.MoveToWarehouse();
    }
}
