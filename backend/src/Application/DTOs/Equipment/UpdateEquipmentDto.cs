namespace Application.DTOs.Equipment
{
    public class UpdateEquipmentDto
    {
        public string? Name { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid SectionId { get; set; }
    }
}
