namespace Application.DTOs.TransferRequest
{
    public class TransferRequestDto
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public Guid TargetDepartmentId { get; set; }
        public Guid RequesterId { get; set; }
        public DateTime RequestedTransferDate { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public Guid? ResolverId { get; set; }
    }
}
