using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
    }
}