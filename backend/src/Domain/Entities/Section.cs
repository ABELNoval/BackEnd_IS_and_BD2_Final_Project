namespace Domain.Entities
{
    public class Section
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Department> Departaments{ get; set; } = new List<Department>();
    }   
}