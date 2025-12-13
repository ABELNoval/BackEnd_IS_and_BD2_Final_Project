namespace Application.DTOs.Maintenance
{
    public class MaintenanceDto
    {
        public Guid Id { get; set; }

        public Guid EquipmentId { get; set; }

        public Guid TechnicalId { get; set; }

        public DateTime MaintenanceDate { get; set; }

        public int MaintenanceTypeId { get; set; }

        // public string MaintenanceTypeName { get; set; } = string.Empty;
        
        public decimal Cost { get; set; }

        // For More Information
        // public string? EquipmentName { get; set; }
        // public string? EquipmentType { get; set; }
        // public string? TechnicalName { get; set; }
    }
}