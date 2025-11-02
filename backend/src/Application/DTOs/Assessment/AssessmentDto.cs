namespace Application.DTOs.Assessment
{
    public class AssessmentDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public double Punctuation { get; set; } 
        public DateTime Date { get; set; }


        public int DirectorId { get; set; }
        public string DirectorName { get; set; } = string.Empty;


        public int TechnicalId { get; set; }
        public string TechnicalName { get; set; } = string.Empty;
        public string TechnicalSpecialty { get; set; } = string.Empty;

    }
}