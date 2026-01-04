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
        Task<IEnumerable<DirectorDto>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}
