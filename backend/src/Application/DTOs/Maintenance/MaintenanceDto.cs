namespace Application.DTOs.Maintenance
{
    public class MaintenanceDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public string Type { get; set; } = string.Empty;
        
        
        public int EquimentId { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public string EquipmentType { get; set; } = string.Empty;
        public string EquipmentState { get; set; } = string.Empty;
        
        
        public int TechnicalId { get; set; }
        public string TechnicalName { get; set; } = string.Empty;
    }
}