namespace Application.DTOs.Assessment
{
    public class UpdateAssessmentDto
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;   
        public float Punctuation { get; set; }
        public DateTime Date { get; set; }
    }
}