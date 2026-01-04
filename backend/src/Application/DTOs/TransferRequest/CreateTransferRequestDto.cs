namespace Application.DTOs.TransferRequest
{
    public class CreateTransferRequestDto
    {
        public Guid EquipmentId { get; set; }
        public Guid TargetDepartmentId { get; set; }
        public DateTime RequestedTransferDate { get; set; }
    }
}
