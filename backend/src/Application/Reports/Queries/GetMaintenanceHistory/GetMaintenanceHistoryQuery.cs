using Application.DTOs.ReportResult;
using MediatR;

namespace Application.Reports.Queries.GetMaintenanceHistory
{
    public record GetMaintenanceHistoryQuery(Guid EquipmentId) : IRequest<IEnumerable<EquipmentMaintenanceHistoryDto>>;
}
