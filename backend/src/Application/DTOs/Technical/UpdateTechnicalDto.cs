namespace Application.DTOs.Technical
{
    public class UpdateTechnicalDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Experience { get; set; }
        public string Specialty { get; set; } = string.Empty;
    }
}