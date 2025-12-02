using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TechnicalRepository : BaseRepository<Technical>, ITechnicalRepository
{
    public TechnicalRepository(AppDbContext context) : base(context)
    {
    }

}