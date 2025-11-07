using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context) { }

        public async Task<Department?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            var trimmed = name.Trim();
            return await _context.Departments
                .Where(d => d.Name.ToLower() == trimmed.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Department>> GetBySectionIdAsync(Guid sectionId)
        {
            return await _context.Departments
                .Where(d => d.SectionId == sectionId)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetByResponsibleIdAsync(Guid responsibleId)
        {
            return await _context.Departments
                .Where(d => d.ResponsibleId == responsibleId)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            return await _context.Departments
                .OrderBy(d => d.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}