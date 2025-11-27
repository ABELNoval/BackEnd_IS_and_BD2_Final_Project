namespace Application.DTOs.Section
{
    public class CreateSectionDto
    {
        public string? Name { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
