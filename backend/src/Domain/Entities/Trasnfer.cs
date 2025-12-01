using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Transfer of equipment between departments.
/// Entity within the Equipment aggregate. References other aggregates by ID only.
/// </summary>
public class Transfer : Entity
{
    public Guid EquipmentId { get; private set; }
    public Guid SourceDepartmentId { get; private set; }
    public Guid TargetDepartmentId { get; private set; }
    public Guid ResponsibleId { get; private set; }
    public DateTime TransferDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // EF Core constructor
    private Transfer() { }

    private Transfer(
        Guid equipmentId,
        Guid sourceDepartmentId,
        Guid targetDepartmentId,
        Guid responsibleId,
        DateTime transferDate)
    {
        GenerateId();
        EquipmentId = equipmentId;
        SourceDepartmentId = sourceDepartmentId;
        TargetDepartmentId = targetDepartmentId;
        ResponsibleId = responsibleId;
        TransferDate = transferDate;
        CreatedAt = DateTime.UtcNow;

        Validate();
    }

    /// <summary>
    /// Creates a new validated Transfer instance.
    /// </summary>
    public static Transfer Create(
        Guid equipmentId,
        Guid sourceDepartmentId,
        Guid targetDepartmentId,
        Guid responsibleId,
        DateTime transferDate)
    {
        return new Transfer(
            equipmentId,
            sourceDepartmentId,
            targetDepartmentId,
            responsibleId,
            transferDate);
    }

    private void Validate()
    {
        ValidateEquipmentId();
        ValidateDepartmentIds();
        ValidateResponsibleId();
        ValidateTransferDate();
    }

    private void ValidateEquipmentId()
    {
        if (EquipmentId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Transfer),
                "Equipment ID cannot be empty");
    }

    private void ValidateDepartmentIds()
    {
        if (SourceDepartmentId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Transfer),
                "Source department ID cannot be empty");

        if (TargetDepartmentId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Transfer),
                "Target department ID cannot be empty");

        if (SourceDepartmentId == TargetDepartmentId)
            throw new BusinessRuleViolationException(
                "TransferToSameDepartment",
                $"Cannot transfer to the same department (ID: {SourceDepartmentId})");
    }

    private void ValidateResponsibleId()
    {
        if (ResponsibleId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Transfer),
                "Responsible ID cannot be empty");
    }

    private void ValidateTransferDate()
    {
        if (TransferDate > DateTime.UtcNow.AddDays(1))
            throw new BusinessRuleViolationException(
                "FutureTransferDate",
                $"Transfer date cannot be in the future (provided: {TransferDate:yyyy-MM-dd})");
    }

    // Domain behavior methods

    /// <summary>
    /// Checks if this transfer is to a specific department.
    /// </summary>
    public bool IsTransferTo(Guid departmentId) => TargetDepartmentId == departmentId;

    /// <summary>
    /// Checks if this transfer is from a specific department.
    /// </summary>
    public bool IsTransferFrom(Guid departmentId) => SourceDepartmentId == departmentId;

    /// <summary>
    /// Checks if this transfer involves a specific department.
    /// </summary>
    public bool InvolvesDepartment(Guid departmentId) =>
        SourceDepartmentId == departmentId || TargetDepartmentId == departmentId;

    /// <summary>
    /// Checks if authorized by a specific person.
    /// </summary>
    public bool WasAuthorizedBy(Guid responsibleId) => ResponsibleId == responsibleId;
}