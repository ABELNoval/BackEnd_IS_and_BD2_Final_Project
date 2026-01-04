namespace Application.DTOs.Transfer
{
    public class TransferDto
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public Guid SourceDepartmentId { get; set; }
        public Guid TargetDepartmentId { get; set; }
        public Guid ResponsibleId { get; set; }
        public Guid RecipientId { get; set; } // Recipient (Technical) in target department
        public DateTime TransferDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}