using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents a transfer of equipment between departments.
/// Entity within the Equipment aggregate. References other aggregates by ID only.
/// </summary>
public class Transfer : Entity
{
    /// <summary>
    /// ID of the equipment being transferred
    /// </summary>
    public Guid EquipmentId { get; private set; }

    /// <summary>
    /// ID of the source department
    /// </summary>
    public Guid SourceDepartmentId { get; private set; }

    /// <summary>
    /// ID of the target department
    /// </summary>
    public Guid TargetDepartmentId { get; private set; }



    /// <summary>
    /// ID of the responsible person authorizing the transfer
    /// </summary>
    public Guid ResponsibleId { get; private set; }

    /// <summary>
    /// ID of the recipient (receiver) in the target department
    /// </summary>
    public Guid RecipientId { get; private set; }

    /// <summary>
    /// Date when the transfer occurred
    /// </summary>
    public DateTime TransferDate { get; private set; }

    /// <summary>
    /// Date and time when this transfer record was created
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    private Transfer() { }

    /// <summary>
    /// Private constructor with validation (Always-Valid pattern)
    /// </summary>
    private Transfer(
        Guid equipmentId,
        Guid sourceDepartmentId,
        Guid targetDepartmentId,
        Guid responsibleId,
        Guid recipientId,
        DateTime transferDate)
    {
        GenerateId();
        ValidateGuidProperty(equipmentId, "Equipment ID");
        ValidateDepartmentIds(sourceDepartmentId, targetDepartmentId);
        ValidateGuidProperty(responsibleId, "Responsible ID");
        ValidateGuidProperty(recipientId, "Recipient ID");
        ValidateTransferDate(transferDate);
        
        EquipmentId = equipmentId;
        SourceDepartmentId = sourceDepartmentId;
        TargetDepartmentId = targetDepartmentId;
        ResponsibleId = responsibleId;
        RecipientId = recipientId;
        TransferDate = transferDate;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Creates a new Transfer instance
    /// </summary>
    /// <param name="equipmentId">ID of the equipment being transferred</param>
    /// <param name="sourceDepartmentId">ID of the source department</param>
    /// <param name="targetDepartmentId">ID of the target department</param>
    /// <param name="responsibleId">ID of the responsible person authorizing the transfer</param>
    /// <param name="transferDate">Date when the transfer occurred</param>
    /// <returns>A new valid Transfer instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public static Transfer Create(
        Guid equipmentId,
        Guid sourceDepartmentId,
        Guid targetDepartmentId,
        Guid responsibleId,
        Guid recipientId,
        DateTime transferDate)
        => new(equipmentId, sourceDepartmentId, targetDepartmentId, responsibleId, recipientId, transferDate);

    /// <summary>
    /// Updates transfer date and responsible person atomically
    /// </summary>
    /// <param name="newTransferDate">The new transfer date</param>
    /// <param name="newResponsibleId">The new responsible person ID</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateBasicInfo(DateTime newTransferDate)
    {
        ValidateTransferDate(newTransferDate);
        
        TransferDate = newTransferDate;
    }

    /// <summary>
    /// Checks if this transfer is to a specific department
    /// </summary>
    /// <param name="departmentId">The department ID to check</param>
    /// <returns>True if the transfer is to the given department; otherwise false</returns>
    public bool IsTransferTo(Guid departmentId) => TargetDepartmentId == departmentId;

    /// <summary>
    /// Checks if this transfer is from a specific department
    /// </summary>
    /// <param name="departmentId">The department ID to check</param>
    /// <returns>True if the transfer is from the given department; otherwise false</returns>
    public bool IsTransferFrom(Guid departmentId) => SourceDepartmentId == departmentId;

    /// <summary>
    /// Checks if this transfer involves a specific department
    /// </summary>
    /// <param name="departmentId">The department ID to check</param>
    /// <returns>True if the transfer involves the given department; otherwise false</returns>
    public bool InvolvesDepartment(Guid departmentId) =>
        SourceDepartmentId == departmentId || TargetDepartmentId == departmentId;

    /// <summary>
    /// Checks if authorized by a specific person
    /// </summary>
    /// <param name="responsibleId">The responsible ID to check</param>
    /// <returns>True if the transfer was authorized by the given responsible; otherwise false</returns>
    public bool WasAuthorizedBy(Guid responsibleId) => ResponsibleId == responsibleId;

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
                nameof(Transfer),
                $"{propertyName} cannot be empty");
    }

    /// <summary>
    /// Validates both department IDs are different and valid
    /// </summary>
    /// <param name="sourceDepartmentId">The source department ID</param>
    /// <param name="targetDepartmentId">The target department ID</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateDepartmentIds(Guid sourceDepartmentId, Guid targetDepartmentId)
    {
        ValidateGuidProperty(sourceDepartmentId, "Source Department ID");
        ValidateGuidProperty(targetDepartmentId, "Target Department ID");

        if (sourceDepartmentId == targetDepartmentId)
            throw new BusinessRuleViolationException(
                "TransferToSameDepartment",
                $"Cannot transfer to the same department (ID: {sourceDepartmentId})");
    }

    /// <summary>
    /// Validates the transfer date
    /// </summary>
    /// <param name="transferDate">The date to validate</param>
    /// <exception cref="BusinessRuleViolationException">If validation fails</exception>
    private void ValidateTransferDate(DateTime transferDate)
    {
        if (transferDate > DateTime.UtcNow.AddDays(1))
            throw new BusinessRuleViolationException(
                "FutureTransferDate",
                $"Transfer date cannot be in the future (provided: {transferDate:yyyy-MM-dd})");
    }

    /// <summary>
    /// Validates the entire entity
    /// </summary>
    /// <exception cref="InvalidEntityException">If entity validation fails</exception>
    private void Validate()
    {
        if (Id == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Transfer),
                "Transfer ID cannot be empty");

        ValidateGuidProperty(EquipmentId, "Equipment ID");
        ValidateDepartmentIds(SourceDepartmentId, TargetDepartmentId);
        ValidateGuidProperty(ResponsibleId, "Responsible ID");
        ValidateTransferDate(TransferDate);
    }

    #endregion
}