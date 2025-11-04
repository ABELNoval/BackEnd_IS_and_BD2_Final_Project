namespace Domain.Exceptions;

/// <summary>
/// Exception thrown when trying to perform operations on disposed equipment
/// </summary>
public class EquipmentDisposedException : DomainException
{
    public Guid EquipmentId { get; }

    public EquipmentDisposedException(Guid equipmentId, string operation)
        : base($"Equipment {equipmentId} is disposed. {operation}")
    {
        EquipmentId = equipmentId;
    }
}
