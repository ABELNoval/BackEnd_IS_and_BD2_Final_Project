using Application.DTOs.ReportRequest;

namespace Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<byte[]> GeneratePdfReport(ReportRequestDto request);
        Task<byte[]> GenerateExcelReport(ReportRequestDto request);
        Task<byte[]> GenerateWordReport(ReportRequestDto request);
    }
}