namespace Application.DTOs.Auth
{
    public class AuthUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public Guid? DepartmentId { get; set; }
        public Guid? SectionId { get; set; }
    }
}
