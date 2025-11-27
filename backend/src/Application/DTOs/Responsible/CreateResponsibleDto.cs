namespace Application.DTOs.Responsible
{
    public class CreateResponsibleDto
    {
        public Guid EmployeeId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}