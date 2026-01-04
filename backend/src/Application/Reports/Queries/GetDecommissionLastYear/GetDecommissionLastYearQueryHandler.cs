using Application.DTOs.ReportResult;
using MediatR;
using Application.Interfaces.Repositories;

namespace Application.Reports.Queries.GetDecommissionLastYear
{
    public class GetDecommissionLastYearQueryHandler : IRequestHandler<GetDecommissionLastYearQuery, IEnumerable<EquipmentDecommissionLastYearDto>>
    {
        private readonly IReportQueriesRepository _repository;

        public GetDecommissionLastYearQueryHandler(IReportQueriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EquipmentDecommissionLastYearDto>> Handle(
            GetDecommissionLastYearQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetEquipmentDecommissionLastYearAsync();
        }
    }
}
