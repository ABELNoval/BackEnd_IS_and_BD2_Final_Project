namespace Application.DTOs.EquipmentType
{
    public class UpdateEquipmentTypeDto
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = string.Empty;
    }
}