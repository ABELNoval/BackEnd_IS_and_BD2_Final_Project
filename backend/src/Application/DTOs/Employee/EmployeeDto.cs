namespace Application.DTOs.Employee
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Nombre del rol
        public Guid DepartmentId {get; set; }
    }
}