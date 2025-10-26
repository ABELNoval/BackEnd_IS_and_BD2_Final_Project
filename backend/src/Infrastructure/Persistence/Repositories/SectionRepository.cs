using Domain.Entities;
using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly AppDbContext _context;

        public SectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Section> GetByIdAsync(Guid id)
        {
            return await _context.Sections.FindAsync(id);
        }

        public async Task<IEnumerable<Section>> GetAllAsync()
        {
            return await _context.Sections.ToListAsync();
        }

        public async Task AddAsync(Section section)
        {
            await _context.Sections.AddAsync(section);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Section section)
        {
            _context.Sections.Update(section);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var section = await GetByIdAsync(id);
            if (section != null)
            {
                _context.Sections.Remove(section);
                await _context.SaveChangesAsync();
            }
        }
    }
}