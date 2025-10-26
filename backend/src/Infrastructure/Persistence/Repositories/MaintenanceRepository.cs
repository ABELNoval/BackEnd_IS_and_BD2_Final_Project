using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class MaintenanceRepository : IMaintenanceRepository
    {
        private readonly AppDbContext _context;

        public MaintenanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Maintenance> GetByIdAsync(Guid id)
        {
            return await _context.Maintenances.FindAsync(id);
        }

        public async Task<IEnumerable<Maintenance>> GetAllAsync()
        {
            return await _context.Maintenances.ToListAsync();
        }

        public async Task AddAsync(Maintenance maintenance)
        {
            await _context.Maintenances.AddAsync(maintenance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Maintenance maintenance)
        {
            _context.Maintenances.Update(maintenance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var maintenance = await GetByIdAsync(id);
            if (maintenance != null)
            {
                _context.Maintenances.Remove(maintenance);
                await _context.SaveChangesAsync();
            }
        }
    }
}