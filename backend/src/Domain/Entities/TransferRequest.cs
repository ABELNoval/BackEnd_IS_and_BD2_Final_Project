using Domain.Common;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents a request to transfer equipment between departments.
/// Must be approved by the target department's responsible before becoming a Transfer.
/// </summary>
public class TransferRequest : Entity
{
    /// <summary>
    /// ID of the equipment to be transferred
    /// </summary>
    public Guid EquipmentId { get; private set; }

    /// <summary>
    /// ID of the target department (where equipment will go)
    /// </summary>
    public Guid TargetDepartmentId { get; private set; }

    /// <summary>
    /// ID of the responsible person who requested the transfer
    /// </summary>
    public Guid RequesterId { get; private set; }

    /// <summary>
    /// Requested date for the transfer
    /// </summary>
    public DateTime RequestedTransferDate { get; private set; }

    /// <summary>
    /// Current status of the request (Pending, Accepted, Denied, Cancelled)
    /// </summary>
    public int StatusId { get; private set; }

    /// <summary>
    /// Date and time when this request was created
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Date and time when the status was last updated
    /// </summary>
    public DateTime? ResolvedAt { get; private set; }

    /// <summary>
    /// ID of the responsible who resolved the request (accepted/denied)
    /// </summary>
    public Guid? ResolverId { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    private TransferRequest() { }

    /// <summary>
    /// Private constructor with validation
    /// </summary>
    private TransferRequest(
        Guid equipmentId,
        Guid targetDepartmentId,
        Guid requesterId,
        DateTime requestedTransferDate)
    {
        GenerateId();
        ValidateGuidProperty(equipmentId, "Equipment ID");
        ValidateGuidProperty(targetDepartmentId, "Target Department ID");
        ValidateGuidProperty(requesterId, "Requester ID");

        EquipmentId = equipmentId;
        TargetDepartmentId = targetDepartmentId;
        RequesterId = requesterId;
        RequestedTransferDate = requestedTransferDate;
        StatusId = TransferRequestStatus.Pending.Id;
        CreatedAt = DateTime.UtcNow;
        ResolvedAt = null;
        ResolverId = null;
    }

    /// <summary>
    /// Factory method to create a new transfer request
    /// </summary>
    public static TransferRequest Create(
        Guid equipmentId,
        Guid targetDepartmentId,
        Guid requesterId,
        DateTime requestedTransferDate)
    {
        return new TransferRequest(equipmentId, targetDepartmentId, requesterId, requestedTransferDate);
    }

    /// <summary>
    /// Accept this transfer request
    /// </summary>
    public void Accept(Guid resolverId)
    {
        if (StatusId != TransferRequestStatus.Pending.Id)
            throw new DomainException("Only pending requests can be accepted.");

        ValidateGuidProperty(resolverId, "Resolver ID");

        StatusId = TransferRequestStatus.Accepted.Id;
        ResolverId = resolverId;
        ResolvedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deny this transfer request
    /// </summary>
    public void Deny(Guid resolverId)
    {
        if (StatusId != TransferRequestStatus.Pending.Id)
            throw new DomainException("Only pending requests can be denied.");

        ValidateGuidProperty(resolverId, "Resolver ID");

        StatusId = TransferRequestStatus.Denied.Id;
        ResolverId = resolverId;
        ResolvedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancel this transfer request (only by the requester)
    /// </summary>
    public void Cancel(Guid requesterId)
    {
        if (StatusId != TransferRequestStatus.Pending.Id)
            throw new DomainException("Only pending requests can be cancelled.");

        if (RequesterId != requesterId)
            throw new DomainException("Only the requester can cancel this request.");

        StatusId = TransferRequestStatus.Cancelled.Id;
        ResolvedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Check if the request is still pending
    /// </summary>
    public bool IsPending => StatusId == TransferRequestStatus.Pending.Id;

    /// <summary>
    /// Validates a GUID property
    /// </summary>
    private void ValidateGuidProperty(Guid value, string propertyName)
    {
        if (value == Guid.Empty)
            throw new DomainException($"{propertyName} cannot be empty.");
    }
}
