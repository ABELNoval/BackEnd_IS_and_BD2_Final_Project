using Domain.Entities;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context) { }
    }
}