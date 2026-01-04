using Application.DTOs.ReportResult;
using MediatR;

namespace Application.Reports.Queries.GetEquipmentTransferHistoryBetweenSections
{
    public record GetEquipmentTransferHistoryBetweenSectionsQuery : IRequest<IEnumerable<EquipmentTransferBetweenSectionsDto>>;
}
