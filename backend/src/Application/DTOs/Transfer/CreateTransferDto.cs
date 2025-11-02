namespace Application.DTOs.Transfer
{
    public class CreateTransferDto
    {
        public DateTime Date { get; set; }
        public int OriginDepartmentId { get; set; }
        public int DestinyDepartmentId { get; set; }
        public int EquipmentId { get; set; }
    }
}