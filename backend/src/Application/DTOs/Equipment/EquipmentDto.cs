namespace Application.DTOs.Equipment
{
    public class EquipmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime AcquisitionDate { get; set; }
        public Guid EquipmentTypeId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string State { get; set; } = string.Empty;
        public string LocationType { get; set; } = string.Empty;
        
    }
}