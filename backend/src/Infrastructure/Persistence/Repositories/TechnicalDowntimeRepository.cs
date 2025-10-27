using Domain.Entities;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class TechnicalDowntimeRepository : BaseRepository<TechnicalDowntime>, ITechnicalDowntimeRepository
    {
        public TechnicalDowntimeRepository(AppDbContext context) : base(context) { }
    }
}