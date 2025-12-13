namespace Domain.ValueObjects;

public class DecommissionContext
{
    public Guid? TargetDepartmentId { get; private set; }
    public Guid ResponsibleId { get; private set; }
    public DateTime TransferDate { get; private set; }

    private DecommissionContext(Guid? targetDepartmentId, Guid responsibleId, DateTime transferDate)
    {
        TargetDepartmentId = targetDepartmentId;
        ResponsibleId = responsibleId;
        TransferDate = transferDate;
    }

    public static DecommissionContext ForDepartment(Guid targetDepartmentId, Guid responsibleId, DateTime transferDate)
    {
        if (targetDepartmentId == Guid.Empty)
            throw new ArgumentException("Target department ID is required", nameof(targetDepartmentId));

        if (responsibleId == Guid.Empty)
            throw new ArgumentException("Responsible ID is required", nameof(responsibleId));

        return new DecommissionContext(targetDepartmentId, responsibleId, transferDate);
    }

    public static DecommissionContext ForWarehouse(Guid responsibleId, DateTime transferDate)
    {
        if (responsibleId == Guid.Empty)
            throw new ArgumentException("Responsible ID is required", nameof(responsibleId));

        return new DecommissionContext(null, responsibleId, transferDate);
    }

    public static DecommissionContext ForDisposal()
    {
        return new DecommissionContext(null, Guid.Empty, DateTime.UtcNow);
    }
}
