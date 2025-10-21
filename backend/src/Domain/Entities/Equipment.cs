namespace Domain.Entities
{
    class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public DateTime AcquisitionDate { get; set; }

        public EquipmentType EquipmentType { get; set; } = null!;
        public Department Department { get; set; } = null!;
    }
}