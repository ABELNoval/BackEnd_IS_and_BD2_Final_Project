using System;

namespace Application.Reports.Generators
{
    /// <summary>
    /// Factory interface for resolving the appropriate report generator based on format.
    /// </summary>
    public interface IReportGeneratorFactory
    {
        /// <summary>
        /// Gets the report generator for the specified format.
        /// </summary>
        /// <param name="format">The report format (e.g., "pdf", "excel", "word").</param>
        /// <returns>An <see cref="IReportGenerator"/> instance for the format.</returns>
        /// <exception cref="NotSupportedException">Thrown if the format is not supported.</exception>
        IReportGenerator GetGenerator(string format);
    }
}
