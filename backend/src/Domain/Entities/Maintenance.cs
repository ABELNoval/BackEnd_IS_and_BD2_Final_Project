using Domain.Common;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents equipment maintenance.
/// Records date, type, cost and responsible technical.
/// </summary>
public class Maintenance : Entity
{
    public Guid EquipmentId { get; private set; }
    public Guid TechnicalId { get; private set; }
    public DateTime MaintenanceDate { get; private set; }
    public int MaintenanceTypeId { get; private set; }
    public decimal Cost { get; private set; }

    protected Maintenance() { }

    private Maintenance(
        Guid equipmentId,
        Guid technicalId,
        DateTime maintenanceDate,
        int maintenanceTypeId,
        decimal cost)
    {
        GenerateId();
        EquipmentId = equipmentId;
        TechnicalId = technicalId;
        MaintenanceDate = maintenanceDate;
        MaintenanceTypeId = maintenanceTypeId;
        Cost = cost;

        Validate();
    }

    public static Maintenance Create(
        Guid equipmentId,
        Guid technicalId,
        DateTime maintenanceDate,
        int maintenanceTypeId,
        decimal cost)
    {
        return new Maintenance(
            equipmentId,
            technicalId,
            maintenanceDate,
            maintenanceTypeId,
            cost);
    }

    private void Validate()
    {
        if (Id == Guid.Empty)
            throw new InvalidEntityException(nameof(Maintenance), "Maintenance ID cannot be empty");

        if (EquipmentId == Guid.Empty)
            throw new InvalidEntityException(nameof(Maintenance), "Equipment ID cannot be empty");

        if (TechnicalId == Guid.Empty)
            throw new InvalidEntityException(nameof(Maintenance), "Technical ID cannot be empty");

        if (MaintenanceDate > DateTime.UtcNow)
            throw new InvalidEntityException(nameof(Maintenance), "Maintenance date cannot be in the future");

        var validMaintenanceType = MaintenanceType.FromId(MaintenanceTypeId);
        if (validMaintenanceType == null)
            throw new InvalidEntityException(nameof(Maintenance), $"Invalid maintenance type ID: {MaintenanceTypeId}");

        if (Cost < 0)
            throw new InvalidEntityException(nameof(Maintenance), "Cost cannot be negative");
    }

    public bool WasPerformedBy(Guid technicalId) => TechnicalId == technicalId;
    
    public bool IsForEquipment(Guid equipmentId) => EquipmentId == equipmentId;
    
    public bool HasMaintenanceType(MaintenanceType maintenanceType) => 
        MaintenanceTypeId == maintenanceType.Id;
}