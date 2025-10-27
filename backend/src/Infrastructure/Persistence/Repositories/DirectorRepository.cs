using Domain.Entities;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class DirectorRepository : BaseRepository<Director>, IDirectorRepository
    {
        public DirectorRepository(AppDbContext context) : base(context) { }
    }
}