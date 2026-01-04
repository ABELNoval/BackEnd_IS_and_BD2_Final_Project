using Application.DTOs.ReportResult;
using MediatR;
using Application.Interfaces.Repositories;

namespace Application.Reports.Queries.GetEquipmentSentToDepartment
{
    public class GetEquipmentSentToDepartmentQueryHandler : IRequestHandler<GetEquipmentSentToDepartmentQuery, IEnumerable<EquipmentSentToDepartmentDto>>
    {
        private readonly IReportQueriesRepository _repository;

        public GetEquipmentSentToDepartmentQueryHandler(IReportQueriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EquipmentSentToDepartmentDto>> Handle(
            GetEquipmentSentToDepartmentQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetEquipmentSentToDepartmentAsync(request.DepartmentId);
        }
    }
}
