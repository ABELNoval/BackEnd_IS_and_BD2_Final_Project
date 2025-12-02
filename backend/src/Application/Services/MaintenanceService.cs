using Application.DTOs.Maintenance;
using Application.Interfaces.Services;
using Application.Validators.Maintenance;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateMaintenanceDto> _createValidator;
        private readonly IValidator<UpdateMaintenanceDto> _updateValidator;

        public MaintenanceService(
            IMaintenanceRepository maintenanceRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateMaintenanceDto> createValidator,
            IValidator<UpdateMaintenanceDto> updateValidator)
        {
            _maintenanceRepository = maintenanceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // Crear mantenimiento
        public async Task<MaintenanceDto> CreateAsync(CreateMaintenanceDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Maintenance>(dto);
            await _maintenanceRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MaintenanceDto>(entity);
        }

        // Actualizar mantenimiento
        public async Task<MaintenanceDto?> UpdateAsync(UpdateMaintenanceDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existing = await _maintenanceRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing is null)
                return null;

            _mapper.Map(dto, existing);
            await _maintenanceRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MaintenanceDto>(existing);
        }

        // Eliminar mantenimiento
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _maintenanceRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _maintenanceRepository.DeleteAsync(id, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        // Obtener mantenimiento por Id
        public async Task<MaintenanceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var maintenance = await _maintenanceRepository.GetByIdAsync(id, cancellationToken);
            return maintenance is null ? null : _mapper.Map<MaintenanceDto>(maintenance);
        }

        // Obtener todos los mantenimientos
        public async Task<IEnumerable<MaintenanceDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var maintenances = await _maintenanceRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<MaintenanceDto>>(maintenances);
        }

        public async Task<IEnumerable<MaintenanceDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _maintenanceRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<MaintenanceDto>>(entities);
        }
    }
}