namespace Application.DTOs.Assessment
{
    public class CreateAssessmentDto
    {
        public string Comment { get; set; } = string.Empty;
        public float Punctuation { get; set; } 
        public DateTime Date{ get; set; }
        public int DirectorId { get; set; }
        public int TechnicalId { get; set; }
    }
}