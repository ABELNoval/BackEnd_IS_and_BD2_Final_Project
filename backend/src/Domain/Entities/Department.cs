namespace Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Section Section { get; set; } = null!;
        public List<Equipment> Equipments { get; set; } = new List<Equipment>()!;
        public List<Employee> Employees { get; set; } = null!;
        public Responsible Responsible{ get; set; } = null!;
    }
}