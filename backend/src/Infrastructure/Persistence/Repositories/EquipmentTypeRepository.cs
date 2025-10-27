using Domain.Entities;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class EquipmentTypeRepository : BaseRepository<EquipmentType>, IEquipmentTypeRepository
    {
        public EquipmentTypeRepository(AppDbContext context) : base(context) { }
    }
}