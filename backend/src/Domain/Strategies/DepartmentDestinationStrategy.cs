using Domain.Entities;
using Domain.Enumerations;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Strategies;

/// <summary>
/// Strategy for handling equipment movement to a department.
/// </summary>
public class DepartmentDestinationStrategy : IDestinationStrategy
{
    /// <summary>
    /// Creates a new instance of DepartmentDestinationStrategy.
    /// The target department ID is obtained from the decommission context.
    /// </summary>
    public DepartmentDestinationStrategy()
    {
    }

    public DestinyType DestinyType => DestinyType.Department;

    public void Validate(DecommissionContext context)
    {
        if (context.TargetDepartmentId == null || context.TargetDepartmentId == Guid.Empty)
        {
            throw new InvalidDestinationException(
                DestinyType.Department,
                "Target department ID is required for department destination");
        }
        
        if (context.ResponsibleId == Guid.Empty)
        {
            throw new InvalidDestinationException(
                DestinyType.Department,
                "Responsible ID is required for department destination");
        }
    }

    public void ApplyTo(Equipment equipment, DecommissionContext context)
    {
        //Validate(context);
        
        // Lógica específica para transferir a departamento
        equipment.MoveToDepartment(context.TargetDepartmentId!.Value);
        
        // Nota: El responsable y fecha se usan en el Transfer que se crea,
        // pero eso lo maneja Equipment.AddDecommission() internamente
    }
}
