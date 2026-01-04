namespace Application.DTOs.Maintenance
{
    public class UpdateMaintenanceDto
    {
        public Guid Id { get; set; }

        public decimal Cost { get; set; }

        public int MaintenanceTypeId { get; set; }

        public DateTime MaintenanceDate { get; set; }
    }
}