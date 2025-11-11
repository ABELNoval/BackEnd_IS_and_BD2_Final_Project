using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.Mappers;

namespace Tests
{
    public static class DbContextFactory
    {
        private static AppDbContext? _sharedContext;

        // 🧠 Un solo contexto compartido (simula persistencia entre operaciones)
        public static AppDbContext GetSharedInMemoryContext()
        {
            if (_sharedContext == null)
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("SharedTestDB")
                    .Options;

                _sharedContext = new AppDbContext(options);
            }
            return _sharedContext;
        }

        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MaintenanceMapper>();
            });

            return config.CreateMapper();
        }
    }
}
