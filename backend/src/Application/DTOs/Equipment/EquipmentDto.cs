namespace Application.DTOs.Equipment
{
    public class EquipmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; }  = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid DepartmentId { get; set; }
        public Guid SectionId { get; set; }
    }
}
