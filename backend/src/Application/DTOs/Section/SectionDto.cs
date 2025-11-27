namespace Application.DTOs.Section
{
    public class SectionDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
