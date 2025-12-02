using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TransferRepository : BaseRepository<Transfer>, ITransferRepository
{
    public TransferRepository(AppDbContext context) : base(context)
    {
    }

}