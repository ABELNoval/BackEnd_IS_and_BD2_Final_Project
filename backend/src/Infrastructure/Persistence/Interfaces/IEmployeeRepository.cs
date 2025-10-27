using Domain.Entities;

namespace Infrastructure.Persistence.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // Métodos específicos de Employee si los hay
    }
}