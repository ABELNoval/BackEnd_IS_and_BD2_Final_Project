namespace Application.DTOs.TechnicalDowntime
{
    public class TechnicalDowntimeDto
    {
        public int Id { get; set; }
        public string Cause { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int TechnicalId { get; set; }
        public string TechnicalName { get; set; } = string.Empty;
        public string TechnicalSpecialty { get; set; } = string.Empty;
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public string EquipmentType { get; set; } = string.Empty;
        public string EquipmentState { get; set; } = string.Empty;
        public int DestinyTypeId { get; set; }
        public string DestinyTypeName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string SectionName { get; set; } = string.Empty;
    }
}