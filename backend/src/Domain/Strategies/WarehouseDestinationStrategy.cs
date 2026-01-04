using Domain.Entities;
using Domain.Enumerations;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Strategies;

/// <summary>
/// Strategy for handling equipment movement to warehouse.
/// </summary>
public class WarehouseDestinationStrategy : IDestinationStrategy
{
    public DestinyType DestinyType => DestinyType.Warehouse;
    
    public void Validate(DecommissionContext context)
    {
        if (context.RecipientId == Guid.Empty)
        {
            throw new InvalidDestinationException(
                DestinyType.Warehouse,
                "Responsible ID is required for warehouse destination");
        }
    }
    
    public void ApplyTo(Equipment equipment, DecommissionContext context)
    {
        Validate(context);
        
        // Lógica específica para mover a almacén
        equipment.MoveToWarehouse();
    }
}
