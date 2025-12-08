using Application.DTOs.ReportRequest;
using Application.Interfaces.Services;

namespace Infrastructure.Reports
{
    public class ReportService : IReportService
    {
        private readonly PdfReportGenerator _pdfGenerator;
        private readonly ExcelReportGenerator _excelGenerator;
        private readonly WordReportGenerator _wordGenerator;

        public ReportService(
            PdfReportGenerator pdfGenerator,
            ExcelReportGenerator excelGenerator,
            WordReportGenerator wordGenerator)
        {
            _pdfGenerator = pdfGenerator;
            _excelGenerator = excelGenerator;
            _wordGenerator = wordGenerator;
        }

        public async Task<byte[]> GeneratePdfReport(ReportRequestDto request)
        {
            return await _pdfGenerator.Generate(request);
        }

        public async Task<byte[]> GenerateExcelReport(ReportRequestDto request)
        {
            return await _excelGenerator.Generate(request);
        }

        public async Task<byte[]> GenerateWordReport(ReportRequestDto request)
        {
            return await _wordGenerator.Generate(request);
        }
    }
}