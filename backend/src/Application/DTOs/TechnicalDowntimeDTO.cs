namespace Application.DTOs
{
    public class TechnicalDowntimeDTO
    {
        public int EquipmentId { get; set; }
        public int TechnicalId { get; set; }
        public int DepartmentId { get; set; }
        public int DestinyTypeId { get; set; }
        public string Cause { get; set; } = string.Empty;
    }
}