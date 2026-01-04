using System.Threading.Tasks;
using Application.DTOs.ReportRequest;

namespace Application.Reports.Generators
{
    /// <summary>
    /// Defines a contract for report generators supporting a specific format (e.g., PDF, Excel, Word).
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// Gets the supported report format (e.g., "pdf", "excel", "word").
        /// </summary>
        string SupportedFormat { get; }

        /// <summary>
        /// Generates a report asynchronously for the given request.
        /// </summary>
        /// <param name="request">The report request DTO containing report parameters.</param>
        /// <returns>A byte array representing the generated report file.</returns>
        Task<byte[]> GenerateAsync(ReportRequestDto request);
    }
}
