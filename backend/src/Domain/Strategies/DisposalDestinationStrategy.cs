using Domain.Entities;
using Domain.Enumerations;
using Domain.ValueObjects;

namespace Domain.Strategies;

/// <summary>
/// Strategy for handling equipment disposal.
/// </summary>
public class DisposalDestinationStrategy : IDestinationStrategy
{
    public DestinyType DestinyType => DestinyType.Disposal;
    
    public void Validate(DecommissionContext context)
    {
        // Disposal no requiere validaciones adicionales
        // Cualquier equipo puede ser desechado sin restricciones de datos
    }
    
    public void ApplyTo(Equipment equipment, DecommissionContext context)
    {
        // Lógica específica para desechar
        equipment.MoveToDisposal();
    }
}
