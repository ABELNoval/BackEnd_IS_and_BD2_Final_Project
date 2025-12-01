namespace Application.DTOs.EquipmentDecommission
{
    public class EquipmentDecommissionDto
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public Guid TechnicalId { get; set; }
        public Guid DepartmentId { get; set; }
        public int DestinyTypeId { get; set; }
        public Guid RecipientId { get; set; }
        public DateTime DecommissionDate { get; set; }
        public string Reason { get; set; } = string.Empty;

    }
}