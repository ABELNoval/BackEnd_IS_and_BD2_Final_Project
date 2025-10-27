using Domain.Entities;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class DestinyTypeRepository : BaseRepository<DestinyType>, IDestinyTypeRepository
    {
        public DestinyTypeRepository(AppDbContext context) : base(context) { }
    }
}