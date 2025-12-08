using Application.DTOs.Director;
using Application.Interfaces.Services;
using Application.Validators.Director;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository _directorRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDirectorDto> _createValidator;
        private readonly IValidator<UpdateDirectorDto> _updateValidator;

        public DirectorService(
            IDirectorRepository directorRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateDirectorDto> createValidator,
            IValidator<UpdateDirectorDto> updateValidator)
        {
            _directorRepository = directorRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<DirectorDto> CreateAsync(CreateDirectorDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Crear entidad directamente desde el DTO
            var entity = Director.Create(
                dto.Name,
                Email.Create(dto.Email),
                PasswordHash.Create(dto.Password)
            );

            await _directorRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<DirectorDto>(entity);
        }

        public async Task<DirectorDto?> UpdateAsync(UpdateDirectorDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existing = await _directorRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            // Usar el método de dominio correcto
            existing.Update(
                dto.Name,
                Email.Create(dto.Email),
                PasswordHash.Create(dto.Password)
            );

            await _directorRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<DirectorDto>(existing);
        }


        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _directorRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _directorRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<DirectorDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _directorRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<DirectorDto>(entity);
        }

        public async Task<IEnumerable<DirectorDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _directorRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<DirectorDto>>(entities);
        }

        public async Task<IEnumerable<DirectorDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _directorRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<DirectorDto>>(entities);
        }
    }
}