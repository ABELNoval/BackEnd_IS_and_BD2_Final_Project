using Application.DTOs.Auth;

namespace Application.Interfaces.Security
{
    public interface IJwtProvider
    {
        string GenerateToken(AuthUserDto user);
    }
}
