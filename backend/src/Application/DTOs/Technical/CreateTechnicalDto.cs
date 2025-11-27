namespace Application.DTOs.Technical
{
    public class CreateTechnicalDto
    {
        public Guid EmployeeId { get; set; }
        public string Specialization { get; set; } = string.Empty;
    }
}