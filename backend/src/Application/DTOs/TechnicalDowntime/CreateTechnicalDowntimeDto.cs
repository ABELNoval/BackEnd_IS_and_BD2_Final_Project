namespace  Application.DTOs.TechnicalDowntime
{
    public class CreaTechnicalDowntimeDto
    {
        public string Cause { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int TechnicalId { get; set; }
        public int EquipmentId { get; set; }
        public int DestinyTypeId { get; set; }
        public int DepartmentId { get; set; }
    }
}