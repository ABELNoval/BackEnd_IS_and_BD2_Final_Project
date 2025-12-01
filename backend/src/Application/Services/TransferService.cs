using Application.DTOs.Transfer;
using Application.Interfaces.Services;
using Application.Validators.Transfer;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTransferDto> _createValidator;
        private readonly IValidator<UpdateTransferDto> _updateValidator;

        public TransferService(
            ITransferRepository transferRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateTransferDto> createValidator,
            IValidator<UpdateTransferDto> updateValidator)
        {
            _transferRepository = transferRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // Crear una transferencia
        public async Task<TransferDto> CreateAsync(CreateTransferDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Transfer>(dto);

            await _transferRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransferDto>(entity);
        }

        // Actualizar una transferencia
        public async Task<TransferDto?> UpdateAsync(UpdateTransferDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existing = await _transferRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _transferRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransferDto>(existing);
        }

        // Eliminar una transferencia
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _transferRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _transferRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Obtener una transferencia por Id
        public async Task<TransferDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _transferRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TransferDto>(entity);
        }

        // Obtener todas las transferencias
        public async Task<IEnumerable<TransferDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _transferRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por equipo
        public async Task<IEnumerable<TransferDto>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (equipmentId == Guid.Empty)
            {
                throw new ArgumentException("Equipment ID cannot be empty", nameof(equipmentId));
            }

            var entities = await _transferRepository.GetByEquipmentIdAsync(equipmentId, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por departamento de origen
        public async Task<IEnumerable<TransferDto>> GetBySourceDepartmentIdAsync(Guid sourceDepartmentId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (sourceDepartmentId == Guid.Empty)
            {
                throw new ArgumentException("Source department ID cannot be empty", nameof(sourceDepartmentId));
            }

            var entities = await _transferRepository.GetBySourceDepartmentIdAsync(sourceDepartmentId, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por departamento destino
        public async Task<IEnumerable<TransferDto>> GetByTargetDepartmentIdAsync(Guid targetDepartmentId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (targetDepartmentId == Guid.Empty)
            {
                throw new ArgumentException("Target department ID cannot be empty", nameof(targetDepartmentId));
            }

            var entities = await _transferRepository.GetByTargetDepartmentIdAsync(targetDepartmentId, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por departamento (origen o destino)
        public async Task<IEnumerable<TransferDto>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (departmentId == Guid.Empty)
            {
                throw new ArgumentException("Department ID cannot be empty", nameof(departmentId));
            }

            var entities = await _transferRepository.GetByDepartmentIdAsync(departmentId, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por responsable
        public async Task<IEnumerable<TransferDto>> GetByResponsibleIdAsync(Guid responsibleId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (responsibleId == Guid.Empty)
            {
                throw new ArgumentException("Responsible ID cannot be empty", nameof(responsibleId));
            }

            var entities = await _transferRepository.GetByResponsibleIdAsync(responsibleId, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias por rango de fechas
        public async Task<IEnumerable<TransferDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            // Validación de fechas
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be later than end date", nameof(startDate));
            }

            var entities = await _transferRepository.GetByDateRangeAsync(startDate, endDate, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener transferencias recientes
        public async Task<IEnumerable<TransferDto>> GetRecentTransfersAsync(int count, CancellationToken cancellationToken = default)
        {
            // Validación del conteo
            if (count <= 0)
            {
                throw new ArgumentException("Count must be greater than 0", nameof(count));
            }

            if (count > 100)
            {
                throw new ArgumentException("Count cannot exceed 100", nameof(count));
            }

            var entities = await _transferRepository.GetRecentTransfersAsync(count, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        // Obtener la última transferencia de un equipo
        public async Task<TransferDto?> GetLatestByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (equipmentId == Guid.Empty)
            {
                throw new ArgumentException("Equipment ID cannot be empty", nameof(equipmentId));
            }

            var entity = await _transferRepository.GetLatestByEquipmentIdAsync(equipmentId, cancellationToken);
            return entity == null ? null : _mapper.Map<TransferDto>(entity);
        }

        // Contar transferencias por equipo
        public async Task<int> CountByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (equipmentId == Guid.Empty)
            {
                throw new ArgumentException("Equipment ID cannot be empty", nameof(equipmentId));
            }

            return await _transferRepository.CountByEquipmentIdAsync(equipmentId, cancellationToken);
        }

        // Contar transferencias por departamento
        public async Task<int> CountByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (departmentId == Guid.Empty)
            {
                throw new ArgumentException("Department ID cannot be empty", nameof(departmentId));
            }

            return await _transferRepository.CountByDepartmentIdAsync(departmentId, cancellationToken);
        }
    }
}