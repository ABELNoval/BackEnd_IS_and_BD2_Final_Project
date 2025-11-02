namespace Application.DTOs.Maintenance
{
    public class UpdateMaintenaceDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public string Type { get; set; } = string.Empty;
        public int TechnicalId { get; set; }
    }
}