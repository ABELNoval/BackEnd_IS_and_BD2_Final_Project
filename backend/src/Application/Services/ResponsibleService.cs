using Application.DTOs.Responsible;
using Application.Interfaces.Services;
using Application.Validators.Responsible;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class ResponsibleService : IResponsibleService
    {
        private readonly IResponsibleRepository _responsibleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateResponsibleDto> _createValidator;
        private readonly IValidator<UpdateResponsibleDto> _updateValidator;

        public ResponsibleService(
            IResponsibleRepository responsibleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateResponsibleDto> createValidator,
            IValidator<UpdateResponsibleDto> updateValidator)
        {
            _responsibleRepository = responsibleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<ResponsibleDto> CreateAsync(CreateResponsibleDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Responsible>(dto);
            await _responsibleRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ResponsibleDto>(entity);
        }

        public async Task<ResponsibleDto?> UpdateAsync(UpdateResponsibleDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existing = await _responsibleRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            await _responsibleRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ResponsibleDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _responsibleRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _responsibleRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<ResponsibleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _responsibleRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<ResponsibleDto>(entity);
        }

        public async Task<IEnumerable<ResponsibleDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _responsibleRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ResponsibleDto>>(entities);
        }
    }
}