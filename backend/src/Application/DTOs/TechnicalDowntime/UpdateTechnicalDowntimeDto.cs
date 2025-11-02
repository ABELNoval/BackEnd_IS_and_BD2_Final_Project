namespace Application.DTOs.TechnicalDowntime
{
    public class UpdateTechnicalDowntimeDto
    {
        public int Id { get; set; }
        public string Cause { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int DestinyTypeId { get; set; }
        public int DepartmentId { get; set; }
    }
}