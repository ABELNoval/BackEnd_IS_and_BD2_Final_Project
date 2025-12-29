using Application.DTOs.ReportResult;
using MediatR;

namespace Application.Reports.Queries.GetEquipmentSentToDepartment
{
    public record GetEquipmentSentToDepartmentQuery(Guid DepartmentId) : IRequest<IEnumerable<EquipmentSentToDepartmentDto>>;
}
