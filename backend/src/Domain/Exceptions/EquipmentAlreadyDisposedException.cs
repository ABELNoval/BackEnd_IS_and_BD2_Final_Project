namespace Domain.Exceptions;

/// <summary>
/// Exception thrown when trying to decommission already disposed equipment
/// </summary>
public class EquipmentAlreadyDisposedException : DomainException
{
    public Guid EquipmentId { get; }

    public EquipmentAlreadyDisposedException(Guid equipmentId)
        : base($"Equipment {equipmentId} is already disposed and cannot be decommissioned")
    {
        EquipmentId = equipmentId;
    }
}
