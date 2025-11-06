using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMaintenanceRepository : IRepository<Maintenance>
    {
        // Métodos específicos de Maintenance si los hay
    }
}