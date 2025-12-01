using Domain.Common;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents equipment decommissioning/disposal.
/// Records when equipment is taken out of service.
/// </summary>
public class EquipmentDecommission : Entity
{
    public Guid EquipmentId { get; private set; }
    public Guid TechnicalId { get; private set; }
    public Guid DepartmentId { get; private set; }
    public int DestinyTypeId { get; private set; }
    public Guid RecipientId { get; private set; }
    public DateTime DecommissionDate { get; private set; }
    public string Reason { get; private set; } = string.Empty;

    protected EquipmentDecommission() { }

    private EquipmentDecommission(
        Guid equipmentId,
        Guid technicalId,
        Guid departmentId,
        int destinyTypeId,
        Guid recipientId,
        DateTime decommissionDate,
        string reason)
    {
        GenerateId();
        EquipmentId = equipmentId;
        TechnicalId = technicalId;
        DepartmentId = departmentId;
        DestinyTypeId = destinyTypeId;
        RecipientId = recipientId;
        DecommissionDate = decommissionDate;
        Reason = reason.Trim();

        Validate();
    }

    public static EquipmentDecommission Create(
        Guid equipmentId,
        Guid technicalId,
        Guid departmentId,
        int destinyTypeId,
        Guid recipientId,
        DateTime decommissionDate,
        string reason)
    {
        return new EquipmentDecommission(
            equipmentId, 
            technicalId, 
            departmentId, 
            destinyTypeId, 
            recipientId, 
            decommissionDate, 
            reason);
    }

    private void Validate()
    {
        const int MaxReasonLength = 500;

        if (EquipmentId == Guid.Empty)
            throw new InvalidEntityException(nameof(EquipmentDecommission), "Equipment ID cannot be empty");

        if (TechnicalId == Guid.Empty)
            throw new InvalidEntityException(nameof(EquipmentDecommission), "Technical ID cannot be empty");

        if (DepartmentId == Guid.Empty)
            throw new InvalidEntityException(nameof(EquipmentDecommission), "Department ID cannot be empty");

        if (RecipientId == Guid.Empty)
            throw new InvalidEntityException(nameof(EquipmentDecommission), "Recipient ID cannot be empty");

        var validDestinyType = DestinyType.FromId(DestinyTypeId);
        if (validDestinyType == null)
            throw new InvalidEntityException(nameof(EquipmentDecommission), $"Invalid destiny type ID: {DestinyTypeId}");

        if (DecommissionDate > DateTime.UtcNow)
            throw new InvalidEntityException(nameof(EquipmentDecommission), "Decommission date cannot be in the future");

        if (string.IsNullOrWhiteSpace(Reason))
            throw new InvalidEntityException(nameof(EquipmentDecommission), "Reason cannot be empty");

        if (Reason.Length > MaxReasonLength)
            throw new InvalidEntityException(nameof(EquipmentDecommission), $"Reason cannot exceed {MaxReasonLength} characters");
    }

    public bool WasDecommissionedBy(Guid technicalId) => TechnicalId == technicalId;
    
    public bool InvolvesDepartment(Guid departmentId) => DepartmentId == departmentId;
    
    public bool IsEquipmentDecommission(Guid equipmentId) => EquipmentId == equipmentId;
    
    public bool HasDestinyType(DestinyType destinyType) => DestinyTypeId == destinyType.Id;
}
