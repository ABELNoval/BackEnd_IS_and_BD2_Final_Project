using Domain.Entities;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
    }
}