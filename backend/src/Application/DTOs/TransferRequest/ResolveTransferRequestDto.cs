namespace Application.DTOs.TransferRequest
{
    /// <summary>
    /// DTO for resolving (accept/deny/cancel) a transfer request
    /// </summary>
    public class ResolveTransferRequestDto
    {
        public Guid Id { get; set; }
        
        /// <summary>
        /// Action to perform: "accept", "deny", or "cancel"
        /// </summary>
        public string Action { get; set; } = string.Empty;
    }
}
