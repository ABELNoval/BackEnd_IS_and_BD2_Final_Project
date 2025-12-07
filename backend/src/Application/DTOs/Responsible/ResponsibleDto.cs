namespace Application.DTOs.Responsible
{
    public class ResponsibleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid DepartmentId {get; set;}
    }
}