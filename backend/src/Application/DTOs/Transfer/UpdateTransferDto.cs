namespace Application.DTOs.Transfer
{
    public class UpdateTransferDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int DestinyDepartmentId { get; set; }
    }
}