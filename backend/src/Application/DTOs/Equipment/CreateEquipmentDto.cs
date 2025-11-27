namespace Application.DTOs.Equipment
{
    public class CreateEquipmentDto
    {
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty ;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid SectionId { get; set; }
    }
}
