using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EquipmentDecommissionRepository : BaseRepository<EquipmentDecommission>, IEquipmentDecommissionRepository
{
    public EquipmentDecommissionRepository(AppDbContext context) : base(context)
    {
    }
}