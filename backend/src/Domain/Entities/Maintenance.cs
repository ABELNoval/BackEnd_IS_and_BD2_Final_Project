using Domain.Common;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents equipment maintenance.
/// Records date, type, cost and responsible technical who performed the maintenance.
/// </summary>
public class Maintenance : Entity
{
    /// <summary>
    /// ID of the equipment being maintained
    /// </summary>
    public Guid EquipmentId { get; private set; }

    /// <summary>
    /// ID of the technical who performed the maintenance
    /// </summary>
    public Guid TechnicalId { get; private set; }

    /// <summary>
    /// Date when the maintenance was performed
    /// </summary>
    public DateTime MaintenanceDate { get; private set; }

    /// <summary>
    /// Type of maintenance performed (references MaintenanceType enumeration)
    /// </summary>
    public int MaintenanceTypeId { get; private set; }

    /// <summary>
    /// Cost of the maintenance
    /// </summary>
    public decimal Cost { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    protected Maintenance() { }

    /// <summary>
    /// Private constructor with validation (Always-Valid pattern)
    /// </summary>
    private Maintenance(
        Guid equipmentId,
        Guid technicalId,
        DateTime maintenanceDate,
        int maintenanceTypeId,
        decimal cost)
    {
        GenerateId();
        ValidateGuidProperty(equipmentId, "Equipment ID");
        ValidateGuidProperty(technicalId, "Technical ID");
        ValidateMaintenanceDate(maintenanceDate);
        ValidateMaintenanceTypeId(maintenanceTypeId);
        ValidateCost(cost);
        
        EquipmentId = equipmentId;
        TechnicalId = technicalId;
        MaintenanceDate = maintenanceDate;
        MaintenanceTypeId = maintenanceTypeId;
        Cost = cost;
    }

    /// <summary>
    /// Creates a new Maintenance instance
    /// </summary>
    /// <param name="equipmentId">ID of the equipment being maintained</param>
    /// <param name="technicalId">ID of the technical performing the maintenance</param>
    /// <param name="maintenanceDate">Date when maintenance was performed</param>
    /// <param name="maintenanceTypeId">Type of maintenance</param>
    /// <param name="cost">Cost of the maintenance</param>
    /// <returns>A new valid Maintenance instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public static Maintenance Create(
        Guid equipmentId,
        Guid technicalId,
        DateTime maintenanceDate,
        int maintenanceTypeId,
        decimal cost)
        => new(equipmentId, technicalId, maintenanceDate, maintenanceTypeId, cost);

    /// <summary>
    /// Updates the maintenance cost
    /// </summary>
    /// <param name="newCost">The new cost for the maintenance</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateCost(decimal newCost)
    {
        ValidateCost(newCost);
        Cost = newCost;
    }

    /// <summary>
    /// Updates maintenance date, type and cost atomically
    /// </summary>
    /// <param name="newMaintenanceDate">The new maintenance date</param>
    /// <param name="newMaintenanceTypeId">The new maintenance type ID</param>
    /// <param name="newCost">The new cost</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateBasicInfo(DateTime newMaintenanceDate, int newMaintenanceTypeId, decimal newCost)
    {
        ValidateMaintenanceDate(newMaintenanceDate);
        ValidateMaintenanceTypeId(newMaintenanceTypeId);
        ValidateCost(newCost);
        
        MaintenanceDate = newMaintenanceDate;
        MaintenanceTypeId = newMaintenanceTypeId;
        Cost = newCost;
    }

    /// <summary>
    /// Checks if this maintenance was performed by the given technical
    /// </summary>
    /// <param name="technicalId">The technical ID to check</param>
    /// <returns>True if maintenance was performed by the given technical; otherwise false</returns>
    public bool WasPerformedBy(Guid technicalId) => TechnicalId == technicalId;

    /// <summary>
    /// Checks if this maintenance is for the given equipment
    /// </summary>
    /// <param name="equipmentId">The equipment ID to check</param>
    /// <returns>True if maintenance is for the given equipment; otherwise false</returns>
    public bool IsForEquipment(Guid equipmentId) => EquipmentId == equipmentId;

    /// <summary>
    /// Checks if this maintenance has the given maintenance type
    /// </summary>
    /// <param name="maintenanceType">The maintenance type to check</param>
    /// <returns>True if maintenance has the given type; otherwise false</returns>
    public bool HasMaintenanceType(MaintenanceType maintenanceType) =>
        MaintenanceTypeId == maintenanceType.Id;

    #region Validation Methods

    /// <summary>
    /// Validates a generic Guid property
    /// </summary>
    /// <param name="id">The ID to validate</param>
    /// <param name="propertyName">The name of the property being validated</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateGuidProperty(Guid id, string propertyName)
    {
        if (id == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Maintenance),
                $"{propertyName} cannot be empty");
    }

    /// <summary>
    /// Validates the maintenance date property
    /// </summary>
    /// <param name="maintenanceDate">The date to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateMaintenanceDate(DateTime maintenanceDate)
    {
        if (maintenanceDate > DateTime.UtcNow)
            throw new InvalidEntityException(
                nameof(Maintenance),
                "Maintenance date cannot be in the future");
    }

    /// <summary>
    /// Validates the maintenance type ID property
    /// </summary>
    /// <param name="maintenanceTypeId">The maintenance type ID to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateMaintenanceTypeId(int maintenanceTypeId)
    {
        var validMaintenanceType = MaintenanceType.FromId(maintenanceTypeId);
        if (validMaintenanceType == null)
            throw new InvalidEntityException(
                nameof(Maintenance),
                $"Invalid maintenance type ID: {maintenanceTypeId}");
    }

    /// <summary>
    /// Validates the cost property
    /// </summary>
    /// <param name="cost">The cost to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateCost(decimal cost)
    {
        if (cost < 0)
            throw new InvalidEntityException(
                nameof(Maintenance),
                "Cost cannot be negative");
    }

    /// <summary>
    /// Validates the entire entity
    /// </summary>
    /// <exception cref="InvalidEntityException">If entity validation fails</exception>
    private void Validate()
    {
        if (Id == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Maintenance),
                "Maintenance ID cannot be empty");

        ValidateGuidProperty(EquipmentId, "Equipment ID");
        ValidateGuidProperty(TechnicalId, "Technical ID");
        ValidateMaintenanceDate(MaintenanceDate);
        ValidateMaintenanceTypeId(MaintenanceTypeId);
        ValidateCost(Cost);
    }

    #endregion
}