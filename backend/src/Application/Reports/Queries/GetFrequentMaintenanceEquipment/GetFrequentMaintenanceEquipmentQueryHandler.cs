using Application.DTOs.ReportResult;
using MediatR;
using Application.Interfaces.Repositories;

namespace Application.Reports.Queries.GetFrequentMaintenanceEquipment
{
    public class GetFrequentMaintenanceEquipmentQueryHandler : IRequestHandler<GetFrequentMaintenanceEquipmentQuery, IEnumerable<EquipmentReplacementReportDto>>
    {
        private readonly IReportQueriesRepository _repository;

        public GetFrequentMaintenanceEquipmentQueryHandler(IReportQueriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EquipmentReplacementReportDto>> Handle(
            GetFrequentMaintenanceEquipmentQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetFrequentMaintenanceEquipmentAsync();
        }
    }
}
