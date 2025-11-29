namespace Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = "";
        public DateTime Expiration { get; set; }
        public AuthUserDto User { get; set; } = default!;
    }
}
