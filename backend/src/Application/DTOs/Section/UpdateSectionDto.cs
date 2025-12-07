namespace Application.DTOs.Section
{
    public class UpdateSectionDto
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public Guid ResponsibleId { get; set; } // new: allow updating responsible of the section
    }
}