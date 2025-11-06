using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class TechnicalDowntimeRepository : BaseRepository<EquipmentDecommission>, ITechnicalDowntimeRepository
    {
        public TechnicalDowntimeRepository(AppDbContext context) : base(context) { }
    }
}