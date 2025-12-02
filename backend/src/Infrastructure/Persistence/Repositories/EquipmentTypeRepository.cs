using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EquipmentTypeRepository : BaseRepository<EquipmentType>, IEquipmentTypeRepository
{
    public EquipmentTypeRepository(AppDbContext context) : base(context)
    {
    }

}