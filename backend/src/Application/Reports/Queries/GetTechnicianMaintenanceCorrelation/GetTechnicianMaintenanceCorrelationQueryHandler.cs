using Application.DTOs.ReportResult;
using MediatR;
using Application.Interfaces.Repositories;

namespace Application.Reports.Queries.GetTechnicianMaintenanceCorrelation
{
    public class GetTechnicianMaintenanceCorrelationQueryHandler : IRequestHandler<GetTechnicianMaintenanceCorrelationQuery, IEnumerable<TechnicianPerformanceCorrelationDto>>
    {
        private readonly IReportQueriesRepository _repository;

        public GetTechnicianMaintenanceCorrelationQueryHandler(IReportQueriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TechnicianPerformanceCorrelationDto>> Handle(
            GetTechnicianMaintenanceCorrelationQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetTechnicianMaintenanceCorrelationAsync();
        }
    }
}
