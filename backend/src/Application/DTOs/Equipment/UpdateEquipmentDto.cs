namespace Application.DTOs.Equipment
{
    public class UpdateEquipmentDto
    {
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid DepartmentId { get; set; }
        public Guid SectionId { get; set; }
    }
}
