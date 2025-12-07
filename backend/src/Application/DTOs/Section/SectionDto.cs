namespace Application.DTOs.Section
{
    public class SectionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ResponsibleId { get; set; } // new: who is responsible for the section
    }
}