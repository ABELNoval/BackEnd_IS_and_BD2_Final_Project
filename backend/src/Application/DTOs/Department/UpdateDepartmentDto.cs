namespace Application.DTOs.Department
{
    public class UpdateDepartmentDto
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public Guid SectionId { get; set; }
    }
}