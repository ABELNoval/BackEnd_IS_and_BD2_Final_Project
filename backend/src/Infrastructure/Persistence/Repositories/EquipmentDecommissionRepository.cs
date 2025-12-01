using Domain.Entities;
using Domain.Enumerations;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TechnicalDowntimeRepository : BaseRepository<EquipmentDecommission>, ITechnicalDowntimeRepository
{
    public TechnicalDowntimeRepository(AppDbContext context) : base(context)
    {
    }
}