namespace Application.DTOs.Maintenance
{
    public class MaintenanceDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public float Cost { get; set; }
        public string Type { get; set; } = string.Empty;
        public int EquimentId { get; set; }
        public string EquimentName { get; set; } = string.Empty;
        public string EquimentType { get; set; } = string.Empty;
        public string EquimentState { get; set; } = string.Empty;
        public int TechnicalId { get; set; }
        public string TechnicalName { get; set; } = string.Empty;
        public string TechnicalSpecialty { get; set; } = string.Empty;
        public int TechnicalExperience { get; set; } 
    }
}