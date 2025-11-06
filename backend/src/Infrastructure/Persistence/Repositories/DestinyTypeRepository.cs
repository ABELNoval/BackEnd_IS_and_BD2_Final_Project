using Domain.Enumerations;
using Domain.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class DestinyTypeRepository : BaseRepository<DestinyType>, IDestinyTypeRepository
    {
        public DestinyTypeRepository(AppDbContext context) : base(context) { }
    }
}