using Application.DTOs.ReportRequest;

namespace Application.Interfaces.Services
{
    /// <summary>
    /// Provides a unified interface for generating reports in various formats.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Generates a report in the specified format asynchronously.
        /// </summary>
        /// <param name="request">The report request DTO containing report parameters.</param>
        /// <param name="format">The desired report format (e.g., "pdf", "excel", "word").</param>
        /// <returns>A byte array representing the generated report file.</returns>
        Task<byte[]> GenerateReportAsync(ReportRequestDto request, string format);
    }
}