using Domain.Enumerations;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class DestinyTypeRepository : BaseRepository<DestinyType>, IDestinyTypeRepository
    {
        public DestinyTypeRepository(AppDbContext context) : base(context) { }

    
    }
}