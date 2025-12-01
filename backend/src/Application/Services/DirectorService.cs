using Application.DTOs.Director;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository _directorRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DirectorService(
            IDirectorRepository directorRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _directorRepository = directorRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DirectorDto> CreateAsync(CreateDirectorDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Director>(dto);
            await _directorRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<DirectorDto>(entity);
        }

        public async Task<DirectorDto?> UpdateAsync(UpdateDirectorDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _directorRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _directorRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<DirectorDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _directorRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _directorRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<DirectorDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _directorRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<DirectorDto>(entity);
        }

        public async Task<IEnumerable<DirectorDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _directorRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<DirectorDto>>(entities);
        }

        public async Task<DirectorDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var entity = await _directorRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<DirectorDto>(entity);
        }

        public async Task<DirectorDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var entity = await _directorRepository.GetByEmailAsync(email, cancellationToken);
            return entity == null ? null : _mapper.Map<DirectorDto>(entity);
        }

        public async Task<IEnumerable<DirectorDto>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await _directorRepository.GetAllPagedAsync(pageNumber, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<DirectorDto>>(entities);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _directorRepository.ExistsByEmailAsync(email, cancellationToken);
        }
    }
}