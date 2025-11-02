namespace Application.DTOs.Maintenance
{
    public class CreateMaintenanceDto
    {
        public DateTime DateTime { get; set; }
        public float Cost { get; set; }
        public string Type { get; set; } = string.Empty;
        public int EquimentId { get; set; }
        public int TechnicalId { get; set; }
    }
}