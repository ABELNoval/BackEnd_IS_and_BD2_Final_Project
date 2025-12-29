namespace Application.DTOs.EquipmentDecommission;

/// <summary>
/// DTO for releasing equipment from warehouse to a department
/// </summary>
public class ReleaseToDepartmentDto
{
    /// <summary>
    /// The target department ID where the equipment will be assigned
    /// </summary>
    public Guid TargetDepartmentId { get; set; }

    /// <summary>
    /// The recipient ID (employee who will receive the equipment)
    /// </summary>
    public Guid RecipientId { get; set; }
}
