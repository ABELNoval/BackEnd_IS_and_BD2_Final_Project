using Domain.Common;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents equipment decommissioning/disposal.
/// Records when equipment is taken out of service and its final destination.
/// </summary>
public class EquipmentDecommission : Entity
{
    private const int MaxReasonLength = 500;
    private const int MinReasonLength = 10;

    /// <summary>
    /// ID of the equipment being decommissioned
    /// </summary>
    public Guid EquipmentId { get; private set; }

    /// <summary>
    /// ID of the technical who performed the decommissioning
    /// </summary>
    public Guid TechnicalId { get; private set; }

    /// <summary>
    /// ID of the department (only for department destiny)
    /// </summary>
    public Guid DepartmentId { get; private set; }

    /// <summary>
    /// Type of destiny for the decommissioned equipment
    /// </summary>
    public int DestinyTypeId { get; private set; }

    /// <summary>
    /// ID of the person receiving/responsible for the equipment
    /// </summary>
    public Guid RecipientId { get; private set; }

    /// <summary>
    /// Date when decommissioning occurred
    /// </summary>
    public DateTime DecommissionDate { get; private set; }

    /// <summary>
    /// Reason for decommissioning
    /// </summary>
    public string Reason { get; private set; } = string.Empty;

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    protected EquipmentDecommission() { }

    /// <summary>
    /// Private constructor with validation (Always-Valid pattern)
    /// </summary>
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
        ValidateGuidProperty(equipmentId, "Equipment ID");
        ValidateGuidProperty(technicalId, "Technical ID");
        ValidateDestinyTypeId(destinyTypeId);
        ValidateDepartmentIdByDestiny(departmentId, destinyTypeId);
        ValidateGuidProperty(recipientId, "Recipient ID");
        ValidateDecommissionDate(decommissionDate);
        ValidateReason(reason);
        
        EquipmentId = equipmentId;
        TechnicalId = technicalId;
        DepartmentId = departmentId;
        DestinyTypeId = destinyTypeId;
        RecipientId = recipientId;
        DecommissionDate = decommissionDate;
        Reason = reason.Trim();
    }

    /// <summary>
    /// Creates a new EquipmentDecommission instance
    /// </summary>
    /// <param name="equipmentId">ID of the equipment being decommissioned</param>
    /// <param name="technicalId">ID of the technical who performed the decommissioning</param>
    /// <param name="departmentId">ID of the department (only for department destiny)</param>
    /// <param name="destinyTypeId">Type of destiny for the equipment</param>
    /// <param name="recipientId">ID of the person receiving the equipment</param>
    /// <param name="decommissionDate">Date when decommissioning occurred</param>
    /// <param name="reason">Reason for decommissioning</param>
    /// <returns>A new valid EquipmentDecommission instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public static EquipmentDecommission Create(
        Guid equipmentId,
        Guid technicalId,
        Guid departmentId,
        int destinyTypeId,
        Guid recipientId,
        DateTime decommissionDate,
        string reason)
        => new(equipmentId, technicalId, departmentId, destinyTypeId, recipientId, decommissionDate, reason);

    /// <summary>
    /// Updates the reason for decommissioning
    /// </summary>
    /// <param name="newReason">The new reason for decommissioning</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateReason(string newReason)
    {
        ValidateReason(newReason);
        Reason = newReason.Trim();
    }

    /// <summary>
    /// Checks if this decommission was performed by the given technical
    /// </summary>
    /// <param name="technicalId">The technical ID to check</param>
    /// <returns>True if the decommission was performed by the given technical; otherwise false</returns>
    public bool WasDecommissionedBy(Guid technicalId) => TechnicalId == technicalId;

    /// <summary>
    /// Checks if this decommission involves a specific department
    /// </summary>
    /// <param name="departmentId">The department ID to check</param>
    /// <returns>True if the decommission involves the given department; otherwise false</returns>
    public bool InvolvesDepartment(Guid departmentId) => DepartmentId == departmentId;

    /// <summary>
    /// Checks if this is a decommission for specific equipment
    /// </summary>
    /// <param name="equipmentId">The equipment ID to check</param>
    /// <returns>True if this is a decommission for the given equipment; otherwise false</returns>
    public bool IsEquipmentDecommission(Guid equipmentId) => EquipmentId == equipmentId;

    /// <summary>
    /// Checks if this decommission has a specific destiny type
    /// </summary>
    /// <param name="destinyType">The destiny type to check</param>
    /// <returns>True if this decommission has the given destiny type; otherwise false</returns>
    public bool HasDestinyType(DestinyType destinyType) => DestinyTypeId == destinyType.Id;

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
                nameof(EquipmentDecommission),
                $"{propertyName} cannot be empty");
    }

    /// <summary>
    /// Validates the destiny type ID
    /// </summary>
    /// <param name="destinyTypeId">The destiny type ID to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateDestinyTypeId(int destinyTypeId)
    {
        var validDestinyType = DestinyType.FromId(destinyTypeId);
        if (validDestinyType == null)
            throw new InvalidEntityException(
                nameof(EquipmentDecommission),
                $"Invalid destiny type ID: {destinyTypeId}");
    }

    /// <summary>
    /// Validates that department ID is set only for department destiny
    /// </summary>
    /// <param name="departmentId">The department ID to validate</param>
    /// <param name="destinyTypeId">The destiny type ID</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateDepartmentIdByDestiny(Guid departmentId, int destinyTypeId)
    {
        var validDestinyType = DestinyType.FromId(destinyTypeId);
        if (validDestinyType == null) return; // Already validated by ValidateDestinyTypeId

        if (validDestinyType.Id == DestinyType.Department.Id)
        {
            if (departmentId == Guid.Empty)
                throw new InvalidEntityException(
                    nameof(EquipmentDecommission),
                    "Department ID cannot be empty for department destiny");
        }
        else
        {
            if (departmentId != Guid.Empty)
                throw new InvalidEntityException(
                    nameof(EquipmentDecommission),
                    "Department ID must be empty for disposal/warehouse destiny");
        }
    }

    /// <summary>
    /// Validates the decommission date
    /// </summary>
    /// <param name="decommissionDate">The date to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateDecommissionDate(DateTime decommissionDate)
    {
        if (decommissionDate > DateTime.UtcNow)
            throw new InvalidEntityException(
                nameof(EquipmentDecommission),
                "Decommission date cannot be in the future");
    }

    /// <summary>
    /// Validates the reason property
    /// </summary>
    /// <param name="reason">The reason to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateReason(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new InvalidEntityException(
                nameof(EquipmentDecommission),
                "Reason cannot be empty");

        var trimmedReason = reason.Trim();

        if (trimmedReason.Length < MinReasonLength)
            throw new InvalidEntityException(
                nameof(EquipmentDecommission),
                $"Reason must be at least {MinReasonLength} characters");

        if (trimmedReason.Length > MaxReasonLength)
            throw new InvalidEntityException(
                nameof(EquipmentDecommission),
                $"Reason cannot exceed {MaxReasonLength} characters");
    }

    /// <summary>
    /// Validates the entire entity
    /// </summary>
    /// <exception cref="InvalidEntityException">If entity validation fails</exception>
    private void Validate()
    {
        if (Id == Guid.Empty)
            throw new InvalidEntityException(
                nameof(EquipmentDecommission),
                "Decommission ID cannot be empty");

        ValidateGuidProperty(EquipmentId, "Equipment ID");
        ValidateGuidProperty(TechnicalId, "Technical ID");
        ValidateDestinyTypeId(DestinyTypeId);
        ValidateDepartmentIdByDestiny(DepartmentId, DestinyTypeId);
        ValidateGuidProperty(RecipientId, "Recipient ID");
        ValidateDecommissionDate(DecommissionDate);
        ValidateReason(Reason);
    }

    #endregion
}
