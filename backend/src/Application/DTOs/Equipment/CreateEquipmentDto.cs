namespace Application.DTOs.Equipment
{
    public class CreateEquipmentDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? AcquisitionDate { get; set; } // Auto-set to today if null
        public Guid EquipmentTypeId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid TechnicalId { get; set; } // Technical for initial preventive maintenance
        // StateId auto-set to UnderMaintenance (2)
        // LocationTypeId auto-set based on DepartmentId
    }
}