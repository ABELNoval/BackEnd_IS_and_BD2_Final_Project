using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SectionRepository : BaseRepository<Section>, ISectionRepository
{
    public SectionRepository(AppDbContext context) : base(context)
    {
    }
}