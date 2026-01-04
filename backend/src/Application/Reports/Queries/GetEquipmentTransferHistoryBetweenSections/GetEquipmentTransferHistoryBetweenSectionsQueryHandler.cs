using Application.DTOs.ReportResult;
using MediatR;
using Application.Interfaces.Repositories;

namespace Application.Reports.Queries.GetEquipmentTransferHistoryBetweenSections
{
    public class GetEquipmentTransferHistoryBetweenSectionsQueryHandler : IRequestHandler<GetEquipmentTransferHistoryBetweenSectionsQuery, IEnumerable<EquipmentTransferBetweenSectionsDto>>
    {
        private readonly IReportQueriesRepository _repository;

        public GetEquipmentTransferHistoryBetweenSectionsQueryHandler(IReportQueriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EquipmentTransferBetweenSectionsDto>> Handle(
            GetEquipmentTransferHistoryBetweenSectionsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetEquipmentTransferHistoryBetweenSectionsAsync();
        }
    }
}
