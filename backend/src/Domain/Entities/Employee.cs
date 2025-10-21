namespace Domain.Entities
{
    class Employee:User
    {
        public Department Department { get; set; } = null!;
    }
}