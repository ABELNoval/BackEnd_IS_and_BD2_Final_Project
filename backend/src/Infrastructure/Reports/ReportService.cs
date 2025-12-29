using Application.DTOs.ReportRequest;
using Application.Interfaces.Services;
using Application.Reports.Generators;

namespace Infrastructure.Reports
{
    /// <summary>
    /// Provides report generation using a factory to resolve the appropriate generator by format.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IReportGeneratorFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService"/> class.
        /// </summary>
        /// <param name="factory">The report generator factory.</param>
        public ReportService(IReportGeneratorFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Generates a report in the specified format asynchronously.
        /// </summary>
        /// <param name="request">The report request DTO containing report parameters.</param>
        /// <param name="format">The desired report format (e.g., "pdf", "excel", "word").</param>
        /// <returns>A byte array representing the generated report file.</returns>
        public async Task<byte[]> GenerateReportAsync(ReportRequestDto request, string format)
        {
            var generator = _factory.GetGenerator(format);
            return await generator.GenerateAsync(request);
        }
    }
}