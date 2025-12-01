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
    }
}