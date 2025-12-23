using Application.DTOs.User;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return users.Select(MapToDto);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            return user == null ? null : MapToDto(user);
        }

        public async Task<UserDto?> UpdateRoleAsync(Guid id, int roleId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null) return null;

            user.UpdateRole(roleId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return MapToDto(user);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null) return false;

            await _userRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IEnumerable<UserDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.FilterAsync(query, cancellationToken);
            return users.Select(MapToDto);
        }

        private static UserDto MapToDto(User user)
        {
            var role = Role.FromId(user.RoleId);
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email.Value,
                RoleId = user.RoleId,
                Role = role?.Name ?? "Unknown"
            };
        }
    }
}
