using Domain.Entities;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class TechnicalRepository : BaseRepository<Technical>, ITechnicalRepository
    {
        public TechnicalRepository(AppDbContext context) : base(context) { }
    }
}