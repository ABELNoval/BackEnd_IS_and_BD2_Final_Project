using Application.DTOs.ReportResult;
using MediatR;
using Application.Interfaces.Repositories;

namespace Application.Reports.Queries.GetMaintenanceHistory
{
    public class GetMaintenanceHistoryQueryHandler : IRequestHandler<GetMaintenanceHistoryQuery, IEnumerable<EquipmentMaintenanceHistoryDto>>
    {
        private readonly IReportQueriesRepository _repository;

        public GetMaintenanceHistoryQueryHandler(IReportQueriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EquipmentMaintenanceHistoryDto>> Handle(
            GetMaintenanceHistoryQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetEquipmentMaintenanceHistoryAsync(request.EquipmentId);
        }
    }
}
