using Application.DTOs.ReportResult;
using MediatR;

namespace Application.Reports.Queries.GetTechnicianMaintenanceCorrelation
{
    public record GetTechnicianMaintenanceCorrelationQuery : IRequest<IEnumerable<TechnicianPerformanceCorrelationDto>>;
}
