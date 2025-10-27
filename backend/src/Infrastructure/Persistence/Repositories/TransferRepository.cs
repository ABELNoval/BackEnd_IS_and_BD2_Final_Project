using Domain.Entities;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class TransferRepository : BaseRepository<Transfer>, ITransferRepository
    {
        public TransferRepository(AppDbContext context) : base(context) { }
    }
}