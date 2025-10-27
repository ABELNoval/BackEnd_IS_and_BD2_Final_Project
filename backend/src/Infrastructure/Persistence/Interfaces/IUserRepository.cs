using Domain.Entities;

namespace Infrastructure.Persistence.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        // Métodos específicos de User si los hay
    }
}