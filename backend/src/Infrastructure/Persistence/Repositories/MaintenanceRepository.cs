using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MaintenanceRepository : BaseRepository<Maintenance>, IMaintenanceRepository
{
    public MaintenanceRepository(AppDbContext context) : base(context)
    {
    }
}