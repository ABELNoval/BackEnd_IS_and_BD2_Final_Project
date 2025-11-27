namespace Application.DTOs.Technical
{
    public class TechnicalDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string Specialization { get; set; } = string.Empty;
    }
}