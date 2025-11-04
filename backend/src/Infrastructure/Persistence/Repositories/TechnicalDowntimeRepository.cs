using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class TechnicalDowntimeRepository : BaseRepository<TechnicalDowntime>, ITechnicalDowntimeRepository
    {
        public TechnicalDowntimeRepository(AppDbContext context) : base(context) { }
    }
}