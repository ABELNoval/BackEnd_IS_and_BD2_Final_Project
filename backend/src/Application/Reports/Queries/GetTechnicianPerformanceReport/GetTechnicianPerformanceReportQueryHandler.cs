using Application.DTOs.ReportResult;
using MediatR;
using Application.Interfaces.Repositories;

namespace Application.Reports.Queries.GetTechnicianPerformanceReport
{
    public class GetTechnicianPerformanceReportQueryHandler : IRequestHandler<GetTechnicianPerformanceReportQuery, List<TechnicianPerformanceReportDto>>
    {
        private readonly IReportQueriesRepository _repository;

        public GetTechnicianPerformanceReportQueryHandler(IReportQueriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TechnicianPerformanceReportDto>> Handle(
            GetTechnicianPerformanceReportQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetTechnicianPerformanceReportAsync();
        }
    }
}
