namespace Application.DTOs.Section
{
    public class CreateSectionDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid ResponsibleId { get; set; }
    }
}