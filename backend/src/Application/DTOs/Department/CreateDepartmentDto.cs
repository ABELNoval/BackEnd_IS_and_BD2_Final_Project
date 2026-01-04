namespace Application.DTOs.Department
{
    public class CreateDepartmentDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid SectionId { get; set; }
    }
}