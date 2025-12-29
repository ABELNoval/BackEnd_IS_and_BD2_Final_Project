using System;
using Application.Reports.Generators;
using Infrastructure.Reports.Generators;
// Add the following using if PdfReportGenerator is in a different namespace
// using Application.Reports.Generators.Pdf;

namespace Infrastructure.Reports
{
    /// <summary>
    /// Factory for resolving the appropriate report generator by format.
    /// </summary>
    public class ReportGeneratorFactory : IReportGeneratorFactory
    {
        private readonly PdfReportGenerator _pdf;
        private readonly ExcelReportGenerator _excel;
        private readonly WordReportGenerator _word;

        public ReportGeneratorFactory(PdfReportGenerator pdf, ExcelReportGenerator excel, WordReportGenerator word)
        {
            _pdf = pdf;
            _excel = excel;
            _word = word;
        }

        public IReportGenerator GetGenerator(string format)
        {
            return format.ToLowerInvariant() switch
            {
                "pdf" => _pdf,
                "excel" or "xlsx" => _excel,
                "word" or "docx" => _word,
                _ => throw new ArgumentException($"Unsupported format: {format}")
            };
        }
    }
}
