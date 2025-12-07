namespace Application.DTOs.Responsible
{
    public class UpdateResponsibleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid DepartmentId {get; set;}
    }
}