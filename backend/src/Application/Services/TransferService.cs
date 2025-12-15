using System.Diagnostics;
using Application.DTOs.Transfer;
using Application.Interfaces.Services;
using Application.Validators.Transfer;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Application.Exceptions;

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
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var equipment = await _equipmentRepository.GetByIdAsync(dto.EquipmentId, cancellationToken);
            if (equipment == null)
                throw new EntityNotFoundException(nameof(Equipment), dto.EquipmentId);

            equipment.AddTransfer(
                dto.TargetDepartmentId,
                dto.ResponsibleId,
                dto.TransferDate);

            var transfer = equipment.Transfers.Last();
            await _transferRepository.CreateAsync(transfer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransferDto>(transfer);
        }

        /// <summary>
        /// Updates a transfer record
        /// </summary>
        public async Task<TransferDto?> UpdateAsync(UpdateTransferDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var existing = await _transferRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Transfer), dto.Id);

            existing.UpdateBasicInfo(dto.TransferDate);

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
                throw new EntityNotFoundException(nameof(Transfer), id);

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
            if (entity == null)
                throw new EntityNotFoundException(nameof(Transfer), id);
            return _mapper.Map<TransferDto>(entity);
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