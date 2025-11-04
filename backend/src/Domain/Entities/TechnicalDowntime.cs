namespace Domain.Entities
{
    public class TechnicalDowntime
    {
        public int Id { get; set; }
        public string Cause { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        // explicit FKs 
        public int TechnicalId { get; set; }
        public int EquipmentId { get; set; }
        public int DestinyTypeId { get; set; }
        public int DepartmentId { get; set; }

        // Navigations
        public Technical Technical { get; set; } = null!;
        public Equipment Equipment { get; set; } = null!;
        public DestinyType DestinyType { get; set; } = null!;
        public Department Department { get; set; } = null!;
    }
}