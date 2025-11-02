namespace Application.DTOs.Maintenance
{
    public class CreateMaintenanceDto
    {
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public string Type { get; set; } = string.Empty;
        public int EquipmentId { get; set; }
        public int TechnicalId { get; set; }
    }
}