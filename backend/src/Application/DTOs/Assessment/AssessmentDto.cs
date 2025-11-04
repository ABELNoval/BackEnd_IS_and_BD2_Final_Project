namespace Application.DTOs.Assessment
{
    public class AssessmentDTO
    {
        public Guid Id { get; set; }

        public Guid TechnicalId { get; set; }

        public Guid DirectorId { get; set; }

        public decimal Score { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime AssessmentDate { get; set; }

        // CHECK => FOR MAPPER
        public string? DirectorName { get; set; }
        public string? TechnicalName { get; set; }
        public string? TechnicalSpecialty { get; set; }

    }
}