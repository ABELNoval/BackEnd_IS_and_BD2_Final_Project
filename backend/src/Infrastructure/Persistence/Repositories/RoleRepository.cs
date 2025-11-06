using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context) { }
    }
}