namespace Application.DTOs.Technical
{
    public class CreateTechnicalDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Experience { get; set; }
        public string Specialty { get; set; } = string.Empty;
    }
}