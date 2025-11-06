using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class EquipmentRepository : BaseRepository<Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(AppDbContext context) : base(context) { }
    }
}