namespace Application.DTOs.EquipmentType
{
    public class EquipmentTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int EquipmentCount { get; set; }
    }
}