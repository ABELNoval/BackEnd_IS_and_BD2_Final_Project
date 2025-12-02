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
            await _technicalRepository.CreateAsync(entity, cancellationToken);
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
            await _technicalRepository.UpdateAsync(existing);
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

        public async Task<IEnumerable<TechnicalDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _technicalRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<TechnicalDto>>(entities);
        }
    }
}