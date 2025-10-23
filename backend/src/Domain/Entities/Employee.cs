namespace Domain.Entities
{
    public class Employee:User
    {
        public int? DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
    }
}