namespace Application.DTOs.Transfer
{
    public class UpdateTransferDto
    {
        public Guid Id { get; set; }

        public Guid TargetDepartmentId { get; set; }
        
        public DateTime TransferDate { get; set; }
    }
}