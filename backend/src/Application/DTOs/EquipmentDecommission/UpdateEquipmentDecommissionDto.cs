namespace Application.DTOs.EquipmentDecommission
{
    public class UpdateEquipmentDecommissionDto
    {
        public Guid Id { get; set; }
        public DateTime DecommissionDate { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}