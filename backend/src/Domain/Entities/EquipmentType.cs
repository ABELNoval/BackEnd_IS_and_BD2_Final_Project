
namespace Domain.Entities
{
    
    class EquipmentType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Equipment> Equipments{ get; set; } = new List<Equipment>();
    }
}   