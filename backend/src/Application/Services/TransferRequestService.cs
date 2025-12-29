using Application.DTOs.TransferRequest;
using Application.DTOs.Transfer;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Application.Exceptions;

namespace Application.Services
{
    public class TransferRequestService : ITransferRequestService
    {
        private readonly ITransferRequestRepository _transferRequestRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly ITransferService _transferService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransferRequestService(
            ITransferRequestRepository transferRequestRepository,
            IEquipmentRepository equipmentRepository,
            ITransferService transferService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _transferRequestRepository = transferRequestRepository;
            _equipmentRepository = equipmentRepository;
            _transferService = transferService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new transfer request
        /// </summary>
        public async Task<TransferRequestDto> CreateAsync(CreateTransferRequestDto dto, Guid requesterId, CancellationToken cancellationToken = default)
        {
            // Validate equipment exists
            var equipment = await _equipmentRepository.GetByIdAsync(dto.EquipmentId, cancellationToken);
            if (equipment == null)
                throw new EntityNotFoundException(nameof(Equipment), dto.EquipmentId);

            var entity = TransferRequest.Create(
                dto.EquipmentId,
                dto.TargetDepartmentId,
                requesterId,
                dto.RequestedTransferDate);

            await _transferRequestRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransferRequestDto>(entity);
        }

        /// <summary>
        /// Gets a transfer request by ID
        /// </summary>
        public async Task<TransferRequestDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _transferRequestRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TransferRequestDto>(entity);
        }

        /// <summary>
        /// Gets all transfer requests
        /// </summary>
        public async Task<IEnumerable<TransferRequestDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _transferRequestRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TransferRequestDto>>(entities);
        }

        /// <summary>
        /// Gets transfer requests for a responsible (both their own and those targeting their departments)
        /// </summary>
        public async Task<IEnumerable<TransferRequestDto>> GetForResponsibleAsync(Guid responsibleId, IEnumerable<Guid> departmentIds, CancellationToken cancellationToken = default)
        {
            // Get requests made by this responsible
            var ownRequests = await _transferRequestRepository.GetByRequesterIdAsync(responsibleId, cancellationToken);

            // Get requests targeting their departments (excluding their own requests to avoid duplicates)
            var incomingRequests = await _transferRequestRepository.GetByTargetDepartmentIdsAsync(departmentIds, cancellationToken);

            // Combine and remove duplicates
            var allRequests = ownRequests
                .Union(incomingRequests.Where(r => r.RequesterId != responsibleId))
                .OrderByDescending(r => r.CreatedAt);

            return _mapper.Map<IEnumerable<TransferRequestDto>>(allRequests);
        }

        /// <summary>
        /// Accept a transfer request (creates a Transfer automatically)
        /// </summary>
        public async Task<TransferRequestDto> AcceptAsync(Guid requestId, Guid resolverId, CancellationToken cancellationToken = default)
        {
            var request = await _transferRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (request == null)
                throw new EntityNotFoundException(nameof(TransferRequest), requestId);

            // Get equipment to obtain the source department
            var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId, cancellationToken);
            if (equipment == null)
                throw new EntityNotFoundException(nameof(Equipment), request.EquipmentId);

            if (!equipment.DepartmentId.HasValue)
                throw new InvalidOperationException("Equipment must be assigned to a department before transfer.");

            // Accept the request
            request.Accept(resolverId);
            await _transferRequestRepository.UpdateAsync(request);

            // Create the actual transfer
            var createTransferDto = new CreateTransferDto
            {
                EquipmentId = request.EquipmentId,
                SourceDepartmentId = equipment.DepartmentId.Value,
                TargetDepartmentId = request.TargetDepartmentId,
                ResponsibleId = resolverId,
                TransferDate = request.RequestedTransferDate
            };

            await _transferService.CreateAsync(createTransferDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransferRequestDto>(request);
        }

        /// <summary>
        /// Deny a transfer request
        /// </summary>
        public async Task<TransferRequestDto> DenyAsync(Guid requestId, Guid resolverId, CancellationToken cancellationToken = default)
        {
            var request = await _transferRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (request == null)
                throw new EntityNotFoundException(nameof(TransferRequest), requestId);

            request.Deny(resolverId);
            await _transferRequestRepository.UpdateAsync(request);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransferRequestDto>(request);
        }

        /// <summary>
        /// Cancel a transfer request (only by requester)
        /// </summary>
        public async Task<TransferRequestDto> CancelAsync(Guid requestId, Guid requesterId, CancellationToken cancellationToken = default)
        {
            var request = await _transferRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (request == null)
                throw new EntityNotFoundException(nameof(TransferRequest), requestId);

            request.Cancel(requesterId);
            await _transferRequestRepository.UpdateAsync(request);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransferRequestDto>(request);
        }

        /// <summary>
        /// Filter transfer requests using Dynamic LINQ
        /// </summary>
        public async Task<IEnumerable<TransferRequestDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _transferRequestRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<TransferRequestDto>>(entities);
        }
    }
}
