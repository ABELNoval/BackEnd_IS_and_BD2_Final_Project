using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class DirectorRepository : BaseRepository<Director>, IDirectorRepository
    {
        public DirectorRepository(AppDbContext context) : base(context) { }

        public async Task<Director?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            var trimmed = name.Trim();
            return await _context.Directors
                .Where(d => d.Name.ToLower() == trimmed.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Director?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            var trimmed = email.Trim();
            return await _context.Directors
                .Where(d => d.Email.Value.ToLower() == trimmed.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Director>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            return await _context.Directors
                .OrderBy(d => d.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            var trimmed = email.Trim();
            return await _context.Directors
                .AnyAsync(d => d.Email.Value.ToLower() == trimmed.ToLower(), cancellationToken);
        }
    }
}