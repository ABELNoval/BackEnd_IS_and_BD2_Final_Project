using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class ResponsibleRepository : BaseRepository<Responsible>, IResponsibleRepository
    {
        public ResponsibleRepository(AppDbContext context) : base(context) { }
    }
}