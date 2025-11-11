using Application.DTOs.Maintenance;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Tests
{
    /// <summary>
    /// Runner que permite crear, actualizar y eliminar entidades de mantenimiento usando una base InMemory.
    /// </summary>
    public static class MaintenanceTestRunner
    {
        public static async Task CreateEntitiesAsync()
        {
            Console.WriteLine("🟩 CREANDO ENTIDADES...");

            var context = DbContextFactory.CreateInMemoryContext();
            var mapper = DbContextFactory.CreateMapper();
            var uow = new UnitOfWork(context);
            var repo = new MaintenanceRepository(context);
            var service = new MaintenanceService(repo, uow, mapper);

            for (int i = 1; i <= 3; i++)
            {
                var dto = new CreateMaintenanceDto
                {
                    EquipmentId = Guid.NewGuid(),
                    TechnicalId = Guid.NewGuid(),
                    MaintenanceDate = DateTime.UtcNow.AddDays(-i),
                    MaintenanceTypeId = i,
                    Cost = 1000 + (i * 500)
                };

                var created = await service.CreateAsync(dto);
                Console.WriteLine($"✅ Mantenimiento creado con ID: {created.Id}");
            }

            Console.WriteLine("✅ FINALIZADO CREAR ENTIDADES");
        }

        public static async Task UpdateEntitiesAsync()
        {
            Console.WriteLine("🟨 ACTUALIZANDO ENTIDADES...");

            var context = DbContextFactory.CreateInMemoryContext();
            var mapper = DbContextFactory.CreateMapper();
            var uow = new UnitOfWork(context);
            var repo = new MaintenanceRepository(context);
            var service = new MaintenanceService(repo, uow, mapper);

            var created = await service.CreateAsync(new CreateMaintenanceDto
            {
                EquipmentId = Guid.NewGuid(),
                TechnicalId = Guid.NewGuid(),
                MaintenanceDate = DateTime.UtcNow,
                MaintenanceTypeId = 1,
                Cost = 1000
            });

            Console.WriteLine($"🧾 Creado mantenimiento temporal con ID: {created.Id}");

            await service.UpdateAsync(new UpdateMaintenanceDto
            {
                Id = created.Id,
                Cost = 2000,
                MaintenanceTypeId = 2,
                MaintenanceDate = DateTime.UtcNow.AddDays(1)
            });

            Console.WriteLine($"✏️ Mantenimiento actualizado correctamente.");

            var all = await service.GetAllAsync();
            foreach (var m in all)
                Console.WriteLine($"➡️ ID: {m.Id} | Costo: {m.Cost} | Tipo: {m.MaintenanceTypeId}");

            Console.WriteLine("✅ FINALIZADO ACTUALIZAR ENTIDADES");
        }

        public static async Task DeleteEntitiesAsync()
        {
            Console.WriteLine("🟥 ELIMINANDO ENTIDADES...");

            var context = DbContextFactory.CreateInMemoryContext();
            var mapper = DbContextFactory.CreateMapper();
            var uow = new UnitOfWork(context);
            var repo = new MaintenanceRepository(context);
            var service = new MaintenanceService(repo, uow, mapper);

            var ids = new List<Guid>();

            for (int i = 1; i <= 3; i++)
            {
                var created = await service.CreateAsync(new CreateMaintenanceDto
                {
                    EquipmentId = Guid.NewGuid(),
                    TechnicalId = Guid.NewGuid(),
                    MaintenanceDate = DateTime.UtcNow,
                    MaintenanceTypeId = i,
                    Cost = 800 + (i * 100)
                });

                ids.Add(created.Id);
                Console.WriteLine($"🧾 Creado temporalmente mantenimiento con ID: {created.Id}");
            }

            foreach (var id in ids)
            {
                await service.DeleteAsync(id);
                Console.WriteLine($"🗑️ Eliminado mantenimiento con ID: {id}");
            }

            Console.WriteLine("✅ FINALIZADO ELIMINAR ENTIDADES");
        }
    }
}