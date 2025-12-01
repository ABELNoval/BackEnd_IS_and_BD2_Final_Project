using Application.DTOs.Technical;
using Application.Interfaces.Services;
using Application.Validators.Technical;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class TechnicalService : ITechnicalService
    {
        private readonly ITechnicalRepository _technicalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTechnicalDto> _createValidator;
        private readonly IValidator<UpdateTechnicalDto> _updateValidator;

        public TechnicalService(
            ITechnicalRepository technicalRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateTechnicalDto> createValidator,
            IValidator<UpdateTechnicalDto> updateValidator)
        {
            _technicalRepository = technicalRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<TechnicalDto> CreateAsync(CreateTechnicalDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Technical>(dto);
            await _technicalRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<TechnicalDto>(entity);
        }

        public async Task<TechnicalDto?> UpdateAsync(UpdateTechnicalDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existing = await _technicalRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _technicalRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<TechnicalDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _technicalRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _technicalRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<TechnicalDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _technicalRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TechnicalDto>(entity);
        }

        public async Task<IEnumerable<TechnicalDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _technicalRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<TechnicalDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            // Validación básica del nombre
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Technical name cannot be empty", nameof(name));
            }

            var entity = await _technicalRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<TechnicalDto>(entity);
        }

        public async Task<TechnicalDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
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

            var entity = await _technicalRepository.GetByEmailAsync(email, cancellationToken);
            return entity == null ? null : _mapper.Map<TechnicalDto>(entity);
        }

        public async Task<IEnumerable<TechnicalDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            // Validación de paginación
            if (page < 1)
            {
                throw new ArgumentException("Page number must be greater than 0", nameof(page));
            }

            if (pageSize < 1 || pageSize > 100)
            {
                throw new ArgumentException("Page size must be between 1 and 100", nameof(pageSize));
            }

            var entities = await _technicalRepository.GetAllPagedAsync(page, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<IEnumerable<TechnicalDto>> GetBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default)
        {
            // Validación básica de la especialidad
            if (string.IsNullOrWhiteSpace(specialty))
            {
                throw new ArgumentException("Specialty cannot be empty", nameof(specialty));
            }

            // Validación de formato de especialidad (solo letras y espacios)
            if (!System.Text.RegularExpressions.Regex.IsMatch(specialty, "^[a-zA-Z\\s]+$"))
            {
                throw new ArgumentException("Specialty can only contain letters and spaces", nameof(specialty));
            }

            var entities = await _technicalRepository.GetBySpecialtyAsync(specialty, cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<IEnumerable<TechnicalDto>> GetByMinimumExperienceAsync(int minExperience, CancellationToken cancellationToken = default)
        {
            // Validación de experiencia mínima
            if (minExperience < 0)
            {
                throw new ArgumentException("Minimum experience cannot be negative", nameof(minExperience));
            }

            if (minExperience > 50)
            {
                throw new ArgumentException("Minimum experience cannot exceed 50 years", nameof(minExperience));
            }

            var entities = await _technicalRepository.GetByMinimumExperienceAsync(minExperience, cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<IEnumerable<TechnicalDto>> GetByExperienceRangeAsync(int minExperience, int maxExperience, CancellationToken cancellationToken = default)
        {
            // Validación de rango de experiencia
            if (minExperience < 0)
            {
                throw new ArgumentException("Minimum experience cannot be negative", nameof(minExperience));
            }

            if (maxExperience < 0)
            {
                throw new ArgumentException("Maximum experience cannot be negative", nameof(maxExperience));
            }

            if (minExperience > maxExperience)
            {
                throw new ArgumentException("Minimum experience cannot be greater than maximum experience", nameof(minExperience));
            }

            if (maxExperience > 50)
            {
                throw new ArgumentException("Maximum experience cannot exceed 50 years", nameof(maxExperience));
            }

            var entities = await _technicalRepository.GetByExperienceRangeAsync(minExperience, maxExperience, cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<TechnicalDto?> GetByIdWithAssessmentsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _technicalRepository.GetByIdWithAssessmentsAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TechnicalDto>(entity);
        }

        public async Task<IEnumerable<TechnicalDto>> GetTechnicalsWithMaintenanceAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _technicalRepository.GetTechnicalsWithMaintenanceAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }

        public async Task<int> GetMaintenanceCountAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (technicalId == Guid.Empty)
            {
                throw new ArgumentException("Technical ID cannot be empty", nameof(technicalId));
            }

            return await _technicalRepository.GetMaintenanceCountAsync(technicalId, cancellationToken);
        }

        public async Task<int> GetAssessmentCountAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (technicalId == Guid.Empty)
            {
                throw new ArgumentException("Technical ID cannot be empty", nameof(technicalId));
            }

            return await _technicalRepository.GetAssessmentCountAsync(technicalId, cancellationToken);
        }

        public async Task<decimal?> GetAverageAssessmentScoreAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (technicalId == Guid.Empty)
            {
                throw new ArgumentException("Technical ID cannot be empty", nameof(technicalId));
            }

            return await _technicalRepository.GetAverageAssessmentScoreAsync(technicalId, cancellationToken);
        }
    }
}