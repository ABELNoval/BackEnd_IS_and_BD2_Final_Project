namespace Domain.ValueObjects;

public class DecommissionContext
{
    public Guid? TargetDepartmentId { get; private set; }
    public Guid RecipientId { get; private set; }
    public DateTime TransferDate { get; private set; }

    private DecommissionContext(Guid? targetDepartmentId, Guid recipientId, DateTime transferDate)
    {
        TargetDepartmentId = targetDepartmentId;
        RecipientId = recipientId;
        TransferDate = transferDate;
    }

    public static DecommissionContext ForDepartment(Guid targetDepartmentId, Guid ReceptorId, DateTime transferDate)
    {
        if (targetDepartmentId == Guid.Empty)
            throw new ArgumentException("Target department ID is required", nameof(targetDepartmentId));

        if (ReceptorId == Guid.Empty)
            throw new ArgumentException("receptor ID is required", nameof(RecipientId));

        return new DecommissionContext(targetDepartmentId, ReceptorId, transferDate);
    }

    public static DecommissionContext ForWarehouse(Guid recipientId, DateTime transferDate)
    {
        if (recipientId == Guid.Empty)
            throw new ArgumentException("receptor ID is required", nameof(recipientId));

        return new DecommissionContext(null, recipientId, transferDate);
    }

    public static DecommissionContext ForDisposal()
    {
        return new DecommissionContext(null, Guid.Empty, DateTime.UtcNow);
    }
}
