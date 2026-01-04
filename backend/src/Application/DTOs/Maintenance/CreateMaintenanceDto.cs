namespace Application.DTOs.Maintenance
{
    public class CreateMaintenanceDto
    {
        public Guid EquipmentId { get; set; }

        public Guid TechnicalId { get; set; }

        public DateTime? MaintenanceDate { get; set; } // Auto-set to today if null

        public int MaintenanceTypeId { get; set; }
        
        public decimal Cost { get; set; }
    }
}