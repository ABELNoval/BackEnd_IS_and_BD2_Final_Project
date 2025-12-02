using Application.DTOs.EquipmentDecommission;
using Application.Interfaces.Services;
using Application.Validators.EquipmentDecommission;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class EquipmentDecommissionService : IEquipmentDecommissionService
    {
        private readonly IEquipmentDecommissionRepository _decommissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEquipmentDecommissionDto> _createValidator;
        private readonly IValidator<UpdateEquipmentDecommissionDto> _updateValidator;

        public EquipmentDecommissionService(
            IEquipmentDecommissionRepository decommissionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateEquipmentDecommissionDto> createValidator,
            IValidator<UpdateEquipmentDecommissionDto> updateValidator)
        {
            _decommissionRepository = decommissionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // Crear baja técnica
        public async Task<EquipmentDecommissionDto> CreateAsync(CreateEquipmentDecommissionDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<EquipmentDecommission>(dto);
            await _decommissionRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EquipmentDecommissionDto>(entity);
        }

        // Actualizar baja técnica
        public async Task<EquipmentDecommissionDto?> UpdateAsync(UpdateEquipmentDecommissionDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existing = await _decommissionRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            await _decommissionRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EquipmentDecommissionDto>(existing);
        }

        // Eliminar baja técnica
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _decommissionRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _decommissionRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Obtener baja técnica por Id
        public async Task<EquipmentDecommissionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _decommissionRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentDecommissionDto>(entity);
        }

        // Obtener todas las bajas técnicas
        public async Task<IEnumerable<EquipmentDecommissionDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _decommissionRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDecommissionDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDecommissionDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _decommissionRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDecommissionDto>>(entities);
        }
    }
}