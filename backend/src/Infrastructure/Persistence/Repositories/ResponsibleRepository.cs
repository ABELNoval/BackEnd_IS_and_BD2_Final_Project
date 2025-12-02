using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ResponsibleRepository : BaseRepository<Responsible>, IResponsibleRepository
{
    public ResponsibleRepository(AppDbContext context) : base(context)
    {
    }

}