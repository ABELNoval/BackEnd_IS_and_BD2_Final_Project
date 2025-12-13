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
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTransferDto> _createValidator;
        private readonly IValidator<UpdateTransferDto> _updateValidator;

        public TransferService(
            IEquipmentRepository equipmentRepository,
            ITransferRepository transferRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateTransferDto> createValidator,
            IValidator<UpdateTransferDto> updateValidator)
        {
            _equipmentRepository = equipmentRepository;
            _transferRepository = transferRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Creates a new transfer record by calling Equipment.AddTransfer()
        /// </summary>
        public async Task<TransferDto> CreateAsync(CreateTransferDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Get the aggregate root Equipment
            var equipment = await _equipmentRepository.GetByIdAsync(dto.EquipmentId, cancellationToken);
            if (equipment == null)
                throw new ValidationException($"Equipment with ID {dto.EquipmentId} not found");

            // Equipment creates and manages the Transfer entity
            equipment.AddTransfer(
                dto.TargetDepartmentId,
                dto.ResponsibleId,
                dto.TransferDate);

            // Save the aggregate root, which includes the new transfer
            await _equipmentRepository.UpdateAsync(equipment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return the last transfer added
            var transfer = equipment.Transfers.Last();
            return _mapper.Map<TransferDto>(transfer);
        }

        /// <summary>
        /// Updates a transfer record
        /// </summary>
        public async Task<TransferDto?> UpdateAsync(UpdateTransferDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existing = await _transferRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            existing.UpdateBasicInfo(dto.TransferDate, dto.ResponsibleId);

            await _transferRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransferDto>(existing);
        }

        /// <summary>
        /// Deletes a transfer record
        /// </summary>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _transferRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _transferRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Gets a transfer record by ID
        /// </summary>
        public async Task<TransferDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var entity = await _transferRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TransferDto>(entity);
        }

        /// <summary>
        /// Gets all transfer records
        /// </summary>
        public async Task<IEnumerable<TransferDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _transferRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }

        public async Task<IEnumerable<TransferDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _transferRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<TransferDto>>(entities);
        }
    }
}