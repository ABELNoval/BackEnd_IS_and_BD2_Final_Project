using Application.DTOs.ReportResult;
using MediatR;

namespace Application.Reports.Queries.GetTechnicianPerformanceReport
{
    public record GetTechnicianPerformanceReportQuery : IRequest<List<TechnicianPerformanceReportDto>>;
}
