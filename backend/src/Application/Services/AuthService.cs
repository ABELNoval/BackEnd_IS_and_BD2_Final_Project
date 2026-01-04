using Application.DTOs.Auth;
using Application.Interfaces.Services;
using Application.Interfaces.Security;
using Domain.Interfaces;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository,
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default)
        {
            // Search all users and find by email or name
            var users = await _userRepository.GetAllAsync(cancellationToken);
            
            var user = users.FirstOrDefault(u => 
                u.Email.Value.Equals(dto.Identifier, StringComparison.OrdinalIgnoreCase) ||
                u.Name.Equals(dto.Identifier, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return null;

            // Validate password
            if (!user.PasswordHash.Verify(dto.Password))
                return null;

            // Determine role based on RoleId (matches Role.cs enumeration)
            string role = user.RoleId switch
            {
                1 => "Administrator",
                2 => "Director",
                3 => "Technical",
                4 => "Employee",
                5 => "Responsible",
                6 => "Receptor",
                _ => "User"
            };

            // Convert to DTO
            var authUserDto = new AuthUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email.Value,
                Role = role
            };

            // Populate DepartmentId and SectionId for Employee/Responsible
            if (user is Domain.Entities.Employee employee)
            {
                authUserDto.DepartmentId = employee.DepartmentId;
                
                var department = await _departmentRepository.GetByIdAsync(employee.DepartmentId, cancellationToken);
                if (department != null)
                {
                    authUserDto.SectionId = department.SectionId;
                }
            }

            // Generate token
            var token = _jwtProvider.GenerateToken(authUserDto);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(6),
                User = authUserDto
            };
        }
    }
}
