using Domain.Enumerations;

namespace Domain.Strategies;

/// <summary>
/// Factory que crea la estrategia de destino correcta según el tipo
/// </summary>
public static class DestinationStrategyFactory
{
    /// <summary>
    /// Crea la estrategia apropiada según el tipo de destino
    /// </summary>
    /// <param name="destinyType">Tipo de destino</param>
    /// <returns>Estrategia de destino</returns>
    /// <exception cref="ArgumentException">Si el tipo de destino no es válido</exception>
    public static IDestinationStrategy Create(DestinyType destinyType)
    {
        if (destinyType == null)
            throw new ArgumentNullException(nameof(destinyType));
        
        // Mapeo directo usando el ID del smart enum
        // IDs: 1 = Department, 2 = Disposal, 3 = Warehouse
        return destinyType.Id switch
        {
            1 => new DepartmentDestinationStrategy(),    // Department
            2 => new DisposalDestinationStrategy(),      // Disposal
            3 => new WarehouseDestinationStrategy(),     // Warehouse
            _ => throw new ArgumentException(
                $"Unknown destiny type: {destinyType.Name} (ID: {destinyType.Id})",
                nameof(destinyType))
        };
    }
    
    /// <summary>
    /// Crea la estrategia apropiada según el ID del tipo de destino
    /// </summary>
    public static IDestinationStrategy Create(int destinyTypeId)
    {
        var destinyType = DestinyType.FromId(destinyTypeId);
        
        if (destinyType == null)
            throw new ArgumentException(
                $"Invalid destiny type ID: {destinyTypeId}",
                nameof(destinyTypeId));
        
        return Create(destinyType);
    }
}
