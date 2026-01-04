using Application.DTOs.TransferRequest;

namespace Application.Interfaces.Services
{
    public interface ITransferRequestService
    {
        /// <summary>
        /// Creates a new transfer request
        /// </summary>
        Task<TransferRequestDto> CreateAsync(CreateTransferRequestDto dto, Guid requesterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a transfer request by ID
        /// </summary>
        Task<TransferRequestDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all transfer requests
        /// </summary>
        Task<IEnumerable<TransferRequestDto>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets transfer requests for a responsible (both their own and those targeting their departments)
        /// </summary>
        Task<IEnumerable<TransferRequestDto>> GetForResponsibleAsync(Guid responsibleId, IEnumerable<Guid> departmentIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// Accept a transfer request (creates a Transfer automatically)
        /// </summary>
        Task<TransferRequestDto> AcceptAsync(Guid requestId, Guid resolverId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deny a transfer request
        /// </summary>
        Task<TransferRequestDto> DenyAsync(Guid requestId, Guid resolverId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancel a transfer request (only by requester)
        /// </summary>
        Task<TransferRequestDto> CancelAsync(Guid requestId, Guid requesterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Filter transfer requests using Dynamic LINQ
        /// </summary>
        Task<IEnumerable<TransferRequestDto>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}
