// using Application.DTOs.Auth;
// using Application.Interfaces.Services;
// using AutoMapper;
// using Domain.Interfaces;
// using Application.Interfaces.Security;

// namespace Application.Services
// {
//     public class AuthService : IAuthService
//     {
//         private readonly IUserRepository _userRepository;
//         private readonly IJwtProvider _jwtProvider;
//         private readonly IMapper _mapper;

//         public AuthService(
//             IUserRepository userRepository,
//             IJwtProvider jwtProvider,
//             IMapper mapper)
//         {
//             _userRepository = userRepository;
//             _jwtProvider = jwtProvider;
//             _mapper = mapper;
//         }

//         public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default)
//         {
//             // Buscar por email
//             var user = await _userRepository.GetByEmailAsync(dto.Identifier, cancellationToken);

//             if (user == null)
//                 return null;

//             // Validar contraseña
//             if (!user.PasswordHash.Verify(dto.Password))
//                 return null;

//             // Determinar rol
//             string role = user.RoleId switch
//             {
//                 1 => "Administrator", // ejemplo
//                 _ => user.GetType().Name // Technician, Director, Responsible...
//             };

//             // Convertir a dto
//             var authUserDto = new AuthUserDto
//             {
//                 Id = user.Id,
//                 Name = user.Name,
//                 Email = user.Email.Value,
//                 Role = role
//             };

//             // Generar token
//             var token = _jwtProvider.GenerateToken(authUserDto);

//             return new AuthResponseDto
//             {
//                 Token = token,
//                 Expiration = DateTime.UtcNow.AddHours(6),
//                 User = authUserDto
//             };
//         }
//     }
// }
