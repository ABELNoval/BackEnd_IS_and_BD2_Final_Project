namespace Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string Identifier { get; set; } = ""; // email o username
        public string Password { get; set; } = "";
    }
}
