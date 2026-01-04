namespace Application.DTOs.Assessment
{
    public class CreateAssessmentDto
    {
        public Guid TechnicalId { get; set; }

        public Guid DirectorId { get; set; }

        public decimal Score { get; set; }

        public string Comment { get; set; } = string.Empty;
    }
}