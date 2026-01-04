namespace Domain.Exceptions;

/// <summary>
/// Exception thrown when trying to decommission already decommissioned equipment
/// </summary>
public class EquipmentAlreadyDecommissionedException : DomainException
{
    public Guid EquipmentId { get; }

    public EquipmentAlreadyDecommissionedException(Guid equipmentId)
        : base($"Equipment {equipmentId} is already decommissioned")
    {
        EquipmentId = equipmentId;
    }
}
