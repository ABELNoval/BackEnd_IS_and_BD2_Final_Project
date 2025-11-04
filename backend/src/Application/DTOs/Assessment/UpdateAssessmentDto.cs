namespace Application.DTOs.Assessment
{
    public class UpdateAssessmentDto
    {
        public Guid Id { get; set; }

        public decimal Score { get; set; }

        public string Comment { get; set; } = string.Empty;
    }
}