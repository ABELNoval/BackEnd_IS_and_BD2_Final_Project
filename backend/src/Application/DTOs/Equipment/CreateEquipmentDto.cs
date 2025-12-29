namespace Application.DTOs.Equipment
{
    public class CreateEquipmentDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? AcquisitionDate { get; set; } // Auto-set to today if null
        public Guid EquipmentTypeId { get; set; }
        public Guid? DepartmentId { get; set; }
        public int StateId { get; set; }     
        public int LocationTypeId { get; set; }
    }
}