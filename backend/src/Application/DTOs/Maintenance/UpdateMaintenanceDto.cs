namespace Application.DTOs.Maintenance
{
    public class UpdateMaintenaceDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public float Cost { get; set; }
        public string Type { get; set; } = string.Empty;
        public int TechnicalId { get; set; }
    }
}