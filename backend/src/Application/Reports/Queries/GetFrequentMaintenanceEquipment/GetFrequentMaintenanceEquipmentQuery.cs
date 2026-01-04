using Application.DTOs.ReportResult;
using MediatR;

namespace Application.Reports.Queries.GetFrequentMaintenanceEquipment
{
    public record GetFrequentMaintenanceEquipmentQuery : IRequest<IEnumerable<EquipmentReplacementReportDto>>;
}
