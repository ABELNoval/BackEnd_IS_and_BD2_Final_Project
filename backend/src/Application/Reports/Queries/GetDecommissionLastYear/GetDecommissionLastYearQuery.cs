using Application.DTOs.ReportResult;
using MediatR;

namespace Application.Reports.Queries.GetDecommissionLastYear
{
    // Query sin parámetros
    public record GetDecommissionLastYearQuery : IRequest<IEnumerable<EquipmentDecommissionLastYearDto>>;
}
