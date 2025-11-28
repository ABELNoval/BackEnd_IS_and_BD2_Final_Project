using Application.DTOs.Director;

namespace Application.Interfaces.Services
{
    public interface IDirectorService
    {
        Task<DirectorDto> CreateAsync(CreateDirectorDto dto, CancellationToken cancellationToken = default);
        Task<DirectorDto?> UpdateAsync(UpdateDirectorDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<DirectorDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<DirectorDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<DirectorDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<DirectorDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<DirectorDto>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}