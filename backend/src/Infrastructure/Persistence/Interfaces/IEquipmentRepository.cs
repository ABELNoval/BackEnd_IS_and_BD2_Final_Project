using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistence.Interfaces
{
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        // Métodos específicos de Equipment si los hay
    }
}