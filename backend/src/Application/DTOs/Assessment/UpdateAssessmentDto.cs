namespace Application.DTOs.Assessment
{
    public class UpdateAssessmentDto
    {
        public int Id { get; set; }
        public string Comention { get; set; } = string.Empty;   
        public float Punctuation { get; set; }
        public DateTime Date { get; set; }
    }
}