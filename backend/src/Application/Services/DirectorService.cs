using Application.DTOs.Director;
using Application.Interfaces.Services;
using Application.Validators.Director;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
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
            // Validar DTO usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Director>(dto);
            await _directorRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<DirectorDto>(entity);
        }

        public async Task<DirectorDto?> UpdateAsync(UpdateDirectorDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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

        public async Task<DirectorDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            // Validación básica del nombre
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty", nameof(name));
            }

            var entity = await _directorRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<DirectorDto>(entity);
        }

        public async Task<DirectorDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            // Validación básica del email
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty", nameof(email));
            }

            // Validación de formato de email (básica)
            if (!email.Contains("@"))
            {
                throw new ArgumentException("Invalid email format", nameof(email));
            }

            var entity = await _directorRepository.GetByEmailAsync(email, cancellationToken);
            return entity == null ? null : _mapper.Map<DirectorDto>(entity);
        }

        public async Task<IEnumerable<DirectorDto>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            // Validación de paginación
            if (pageNumber < 1)
            {
                throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
            }

            if (pageSize < 1 || pageSize > 100)
            {
                throw new ArgumentException("Page size must be between 1 and 100", nameof(pageSize));
            }

            var entities = await _directorRepository.GetAllPagedAsync(pageNumber, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<DirectorDto>>(entities);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            // Validación básica del email
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty", nameof(email));
            }

            // Validación de formato de email (básica)
            if (!email.Contains("@"))
            {
                throw new ArgumentException("Invalid email format", nameof(email));
            }

            return await _directorRepository.ExistsByEmailAsync(email, cancellationToken);
        }
    }
}