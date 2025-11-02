namespace Application.DTOs.Transfet
{
    public class CreateTransferDto
    {
        public DateTime DateTime { get; set; }
        public int OriginDepartmentId { get; set; }
        public int DestinyDepartmentId { get; set; }
        public int EquimentId { get; set; }
    }
}