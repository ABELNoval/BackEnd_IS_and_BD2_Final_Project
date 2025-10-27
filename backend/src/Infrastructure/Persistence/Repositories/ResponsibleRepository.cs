using Domain.Entities;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class ResponsibleRepository : BaseRepository<Responsible>, IResponsibleRepository
    {
        public ResponsibleRepository(AppDbContext context) : base(context) { }
    }
}