namespace Application.DTOs.Transfer
{
    public class CreateTransferDto
    {
        public Guid EquipmentId { get; set; }
        public Guid SourceDepartmentId { get; set; }
        public Guid TargetDepartmentId { get; set; }
        public Guid ResponsibleId { get; set; }
        public Guid RecipientId { get; set; } // Recipient (Technical) in target department
        public DateTime? TransferDate { get; set; } // Auto-set to today if null
    }
}

