namespace Application.DTOs.Equipment
{
    public class UpdateEquipmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime AcquisitionDate { get; set; }
        public Guid EquipmentTypeId { get; set; }
        public Guid? DepartmentId { get; set; }
        public int StateId { get; set; }          
        public int LocationTypeId { get; set; }   
    }
}