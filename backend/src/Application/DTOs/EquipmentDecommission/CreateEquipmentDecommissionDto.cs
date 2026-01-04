namespace Application.DTOs.EquipmentDecommission
{
    public class CreateEquipmentDecommissionDto
    {
        public Guid EquipmentId { get; set; }
        public Guid TechnicalId { get; set; }
        public Guid DepartmentId { get; set; }
        public int DestinyTypeId { get; set; }
        public Guid RecipientId { get; set; }
        public DateTime? DecommissionDate { get; set; } // Auto-set to today if null
        public string Reason { get; set; } = string.Empty;
    }
}