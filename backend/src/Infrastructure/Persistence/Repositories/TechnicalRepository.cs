using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class TechnicalRepository : BaseRepository<Technical>, ITechnicalRepository
    {
        public TechnicalRepository(AppDbContext context) : base(context) { }
    }
}